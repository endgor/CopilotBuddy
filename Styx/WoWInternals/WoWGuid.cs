using System;
using System.Runtime.InteropServices;

namespace Styx.WoWInternals
{
    /// <summary>
    /// 64‑bit GUID used by WoW objects.  This struct is a simplified, clean
    /// version of HonorBuddy's implementation, exposing only the fields and
    /// functionality needed by CopilotBuddy.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WoWGuid : IEquatable<WoWGuid>
    {
        public uint Lowest;
        public uint Lower;
        public uint Higher;
        public uint Highest;

        public static readonly WoWGuid Empty = new WoWGuid(0, 0, 0, 0);

        public WoWGuid(uint highest, uint higher, uint lower, uint lowest)
        {
            Highest = highest;
            Higher = higher;
            Lower = lower;
            Lowest = lowest;
        }

        public WoWGuidType Type => (WoWGuidType)(Highest >> 26);
        public bool IsValid => Type > WoWGuidType.None;

        public int RealmId => (int)((Highest >> 10) & 0xFFFFu);
        public int Entry => (int)((Higher >> 6) & 0x7FFFFFu);

        public bool Equals(WoWGuid other)
        {
            return Lowest == other.Lowest && Lower == other.Lower && Higher == other.Higher && Highest == other.Highest;
        }

        public override bool Equals(object obj)
        {
            return obj is WoWGuid wg && Equals(wg);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)Lowest;
                hash = (hash * 397) ^ (int)Lower;
                hash = (hash * 397) ^ (int)Higher;
                hash = (hash * 397) ^ (int)Highest;
                return hash;
            }
        }

        public static bool operator ==(WoWGuid a, WoWGuid b) => a.Equals(b);
        public static bool operator !=(WoWGuid a, WoWGuid b) => !a.Equals(b);

        public override string ToString()
        {
            return string.Format("{0:X8}{1:X8}{2:X8}{3:X8}", Highest, Higher, Lower, Lowest);
        }
    }
}