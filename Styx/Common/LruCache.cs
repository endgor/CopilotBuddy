using System;
using System.Collections.Generic;
using System.Threading;

namespace Styx.Common
{
    /// <summary>
    /// HB 6.2.3: Thread-safe LRU cache with fixed capacity.
    /// Uses a hash table + doubly-linked list for O(1) get and set.
    /// Evicts the least recently used entry when capacity is reached.
    /// </summary>
    public sealed class LruCache<TKey, TValue>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly object _lock = new object();

        // Hash table: bucket index → entry index (-1 = empty)
        private int[] _buckets;
        // Entry array (parallel arrays stored as struct)
        private Entry[] _entries;
        // LRU doubly-linked list heads: _mruHead = most recently used, _lruTail = least recently used
        private int _mruHead;
        private int _lruTail;

        public int Capacity { get; private set; }
        public int Count { get; private set; }

        public LruCache(int capacity, IEqualityComparer<TKey> comparer = null)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _mruHead = -1;
            _lruTail = -1;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!TryGetValue(key, out value))
                    throw new KeyNotFoundException();
                return value;
            }
            set { Set(key, value); }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_lock)
            {
                int index = FindEntry(key);
                if (index != -1)
                {
                    value = _entries[index].Value;
                    MoveToMru(index);
                    return true;
                }
                value = default;
                return false;
            }
        }

        public void Set(TKey key, TValue value)
        {
            lock (_lock)
            {
                EnsureInitialized();
                uint hashCode = (uint)_comparer.GetHashCode(key);
                uint bucket = hashCode % (uint)_buckets.Length;

                // Update existing
                for (int i = _buckets[(int)bucket]; i != -1; i = _entries[i].Next)
                {
                    if (_entries[i].HashCode == hashCode && _comparer.Equals(_entries[i].Key, key))
                    {
                        _entries[i].Value = value;
                        MoveToMru(i);
                        return;
                    }
                }

                // Insert new
                int slot;
                if (Count == Capacity)
                {
                    slot = EvictLru();
                }
                else
                {
                    if (Count == _entries.Length)
                    {
                        Resize();
                        bucket = hashCode % (uint)_buckets.Length;
                    }
                    slot = Count;
                }

                InsertEntry(slot, ref _entries[slot], bucket, hashCode, key, value);
                Count++;
            }
        }

        private void EnsureInitialized()
        {
            if (_buckets != null) return;
            int size = PrimeSizes.NextPrime(3);
            _buckets = new int[size];
            for (int i = 0; i < size; i++) _buckets[i] = -1;
            _entries = new Entry[size];
            _mruHead = -1;
            _lruTail = -1;
        }

        private int FindEntry(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (_buckets == null) return -1;
            uint hashCode = (uint)_comparer.GetHashCode(key);
            for (int i = _buckets[(int)(hashCode % (uint)_buckets.Length)]; i != -1; i = _entries[i].Next)
            {
                if (_entries[i].HashCode == hashCode && _comparer.Equals(_entries[i].Key, key))
                    return i;
            }
            return -1;
        }

        private void MoveToMru(int index)
        {
            int prev = _entries[index].LruPrev;
            int next = _entries[index].LruNext;
            if (prev == -1) return; // already head

            if (prev != -1) _entries[prev].LruNext = next;
            if (next != -1) _entries[next].LruPrev = prev;
            else _lruTail = prev;

            _entries[index].LruPrev = -1;
            _entries[index].LruNext = _mruHead;
            if (_mruHead != -1) _entries[_mruHead].LruPrev = index;
            _mruHead = index;
        }

        private int EvictLru()
        {
            int slot = _lruTail;
            if (Count != 1)
            {
                int newTail = _entries[_lruTail].LruPrev;
                _entries[newTail].LruNext = -1;
                _lruTail = newTail;
            }
            else
            {
                _mruHead = -1;
                _lruTail = -1;
            }
            // Remove from bucket chain
            uint bucketIndex = _entries[slot].HashCode % (uint)_buckets.Length;
            int prev = -1;
            for (int i = _buckets[(int)bucketIndex]; i != -1; i = _entries[i].Next)
            {
                if (i == slot)
                {
                    if (prev == -1) _buckets[(int)bucketIndex] = _entries[i].Next;
                    else _entries[prev].Next = _entries[i].Next;
                    break;
                }
                prev = i;
            }
            Count--;
            return slot;
        }

        private void InsertEntry(int slot, ref Entry entry, uint bucket, uint hashCode, TKey key, TValue value)
        {
            entry.HashCode = hashCode;
            entry.Next = _buckets[(int)bucket];
            entry.Key = key;
            entry.Value = value;
            entry.LruPrev = -1;
            entry.LruNext = _mruHead;
            if (_mruHead != -1) _entries[_mruHead].LruPrev = slot;
            _mruHead = slot;
            if (_lruTail == -1) _lruTail = slot;
            _buckets[(int)bucket] = slot;
        }

        private void Resize()
        {
            uint newSize = (uint)PrimeSizes.NextPrime(Math.Min(Capacity, Count * 2));
            int[] newBuckets = new int[newSize];
            for (int i = 0; i < newBuckets.Length; i++) newBuckets[i] = -1;
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(_entries, newEntries, Count);
            for (int j = 0; j < Count; j++)
            {
                uint b = newEntries[j].HashCode % newSize;
                newEntries[j].Next = newBuckets[(int)b];
                newBuckets[(int)b] = j;
            }
            _buckets = newBuckets;
            _entries = newEntries;
        }

        private struct Entry
        {
            public uint HashCode;
            public int Next;      // next in bucket chain
            public int LruPrev;   // doubly-linked LRU list
            public int LruNext;
            public TKey Key;
            public TValue Value;
        }

        /// <summary>HB 6.2.3 ns91.Class1331 — prime sizes for hash table buckets.</summary>
        private static class PrimeSizes
        {
            private static readonly int[] Primes = {
                3, 7, 11, 17, 23, 29, 37, 47, 59, 71,
                89, 107, 131, 163, 197, 239, 293, 353, 431, 521,
                631, 761, 919, 1103, 1327, 1597, 1931, 2333, 2801, 3371,
                4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023,
                25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363,
                156437, 187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403,
                968897, 1162687, 1395263, 1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559,
                5999471, 7199369
            };

            public static int NextPrime(int min)
            {
                foreach (int p in Primes)
                    if (p >= min) return p;
                return -1;
            }
        }
    }
}
