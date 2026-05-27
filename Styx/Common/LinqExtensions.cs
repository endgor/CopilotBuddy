using System;
using System.Collections.Generic;
using System.Linq;

namespace Styx.Common
{
    /// <summary>
    /// HB 6.2.3: Styx.Common.LinqExtensions
    /// LINQ helper extension methods. Pure C#, no WoW dependencies.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>Inverse of Any(predicate).</summary>
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }

        /// <summary>Returns element with minimum key, or default if sequence is empty.</summary>
        public static TSource MinByOrDefault<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return FindExtreme(source, keySelector, wantMax: false);
        }

        /// <summary>Returns element with maximum key, or default if sequence is empty.</summary>
        public static TSource MaxByOrDefault<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return FindExtreme(source, keySelector, wantMax: true);
        }

        private static T FindExtreme<T, U>(IEnumerable<T> source, Func<T, U> keySelector, bool wantMax)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            int sign = wantMax ? -1 : 1;
            Comparer<U> comparer = Comparer<U>.Default;

            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return default;

                T bestItem = enumerator.Current;
                U bestKey = keySelector(bestItem);

                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;
                    U currentKey = keySelector(current);
                    if (sign * comparer.Compare(currentKey, bestKey) < 0)
                    {
                        bestItem = current;
                        bestKey = currentKey;
                    }
                }
                return bestItem;
            }
        }
    }
}
