using System;

#nullable disable
namespace Styx.Logic.Inventory
{
    /// <summary>
    /// Represents a WoW currency amount in gold, silver, and copper.
    /// Provides arithmetic operations and formatting for prices.
    /// </summary>
    public class WoWPrice : IEquatable<WoWPrice>, IComparable<WoWPrice>
    {
        private readonly long _totalCopper;

        /// <summary>
        /// Creates a new WoWPrice from a copper amount.
        /// </summary>
        public WoWPrice(long copper)
        {
            _totalCopper = copper;
            Gold = copper / 10000L;
            long remainder = copper - Gold * 10000L;
            Silver = remainder / 100L;
            Copper = remainder - Silver * 100L;
        }

        /// <summary>
        /// Creates a new WoWPrice from gold, silver, and copper amounts.
        /// </summary>
        public WoWPrice(long gold, long silver, long copper)
            : this(gold * 10000L + silver * 100L + copper)
        {
        }

        /// <summary>
        /// Gets the total value in copper.
        /// </summary>
        public long TotalCoppers => _totalCopper;

        /// <summary>
        /// Gets the gold component.
        /// </summary>
        public long Gold { get; private set; }

        /// <summary>
        /// Gets the silver component.
        /// </summary>
        public long Silver { get; private set; }

        /// <summary>
        /// Gets the copper component.
        /// </summary>
        public long Copper { get; private set; }

        /// <summary>
        /// Creates a WoWPrice from a gold amount.
        /// </summary>
        public static WoWPrice FromGold(long gold) => new WoWPrice(gold * 10000L);

        /// <summary>
        /// Creates a WoWPrice from a silver amount.
        /// </summary>
        public static WoWPrice FromSilver(long silver) => new WoWPrice(silver * 100L);

        /// <summary>
        /// Creates a WoWPrice representing zero.
        /// </summary>
        public static WoWPrice Zero => new WoWPrice(0L);

        // Arithmetic operators with WoWPrice
        public static WoWPrice operator +(WoWPrice left, WoWPrice right)
            => new WoWPrice(left._totalCopper + right.TotalCoppers);

        public static WoWPrice operator -(WoWPrice left, WoWPrice right)
            => new WoWPrice(left._totalCopper - right.TotalCoppers);

        public static WoWPrice operator *(WoWPrice left, WoWPrice right)
            => new WoWPrice(left._totalCopper * right.TotalCoppers);

        public static WoWPrice operator /(WoWPrice left, WoWPrice right)
            => new WoWPrice(left._totalCopper / right.TotalCoppers);

        // Arithmetic operators with long (copper)
        public static WoWPrice operator +(WoWPrice left, long copper)
            => new WoWPrice(left._totalCopper + copper);

        public static WoWPrice operator -(WoWPrice left, long copper)
            => new WoWPrice(left._totalCopper - copper);

        public static WoWPrice operator *(WoWPrice left, long copper)
            => new WoWPrice(left._totalCopper * copper);

        public static WoWPrice operator /(WoWPrice left, long copper)
            => new WoWPrice(left._totalCopper / copper);

        // Arithmetic operators with double (factor)
        public static WoWPrice operator *(WoWPrice left, double factor)
            => new WoWPrice((long)(left._totalCopper * factor));

        public static WoWPrice operator /(WoWPrice left, double factor)
            => new WoWPrice((long)(left._totalCopper / factor));

        // Comparison operators
        public static bool operator <(WoWPrice left, WoWPrice right)
            => left.TotalCoppers < right.TotalCoppers;

        public static bool operator >(WoWPrice left, WoWPrice right)
            => left.TotalCoppers > right.TotalCoppers;

        public static bool operator <=(WoWPrice left, WoWPrice right)
            => left.TotalCoppers <= right.TotalCoppers;

        public static bool operator >=(WoWPrice left, WoWPrice right)
            => left.TotalCoppers >= right.TotalCoppers;

        public static bool operator ==(WoWPrice left, WoWPrice right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(WoWPrice left, WoWPrice right)
            => !(left == right);

        // Implicit conversion from long
        public static implicit operator WoWPrice(long copper) => new WoWPrice(copper);

        // Explicit conversion to long
        public static explicit operator long(WoWPrice price) => price.TotalCoppers;

        public override string ToString() => $"{Gold}g{Silver}s{Copper}c";

        /// <summary>
        /// Returns a formatted string with color codes (for use in WoW).
        /// </summary>
        public string ToColorString()
        {
            string result = "";
            if (Gold > 0)
                result += $"|cFFFFD700{Gold}g|r ";
            if (Silver > 0 || Gold > 0)
                result += $"|cFFC0C0C0{Silver}s|r ";
            result += $"|cFFB87333{Copper}c|r";
            return result.Trim();
        }

        public bool Equals(WoWPrice other)
        {
            if (other is null)
                return false;
            return _totalCopper == other._totalCopper;
        }

        public override bool Equals(object obj)
            => obj is WoWPrice other && Equals(other);

        public override int GetHashCode() => _totalCopper.GetHashCode();

        public int CompareTo(WoWPrice other)
        {
            if (other is null)
                return 1;
            return _totalCopper.CompareTo(other._totalCopper);
        }
    }
}
