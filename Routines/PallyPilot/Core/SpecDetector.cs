using System;
using Styx;
using Styx.WoWInternals;

namespace PallyPilot.Core
{
    public enum PalSpec { Unknown, Holy, Protection, Retribution }

    /// <summary>
    /// Determines the active paladin spec by counting spent talent points per tab via Lua (the same
    /// technique Singular's TalentManager uses). Cached and re-evaluated on a short throttle so a
    /// respec / dual-spec swap is picked up live without rebuilding the behavior tree — the tree
    /// branches on Current at tick time.
    ///
    /// Paladin talent tabs (3.3.5a): 1 = Holy, 2 = Protection, 3 = Retribution.
    /// Below 10 (no talents) we default to Retribution — the leveling spec.
    /// </summary>
    public static class SpecDetector
    {
        private static PalSpec _current = PalSpec.Unknown;
        private static DateTime _nextCheck = DateTime.MinValue;
        private static readonly TimeSpan Throttle = TimeSpan.FromSeconds(5);

        public static PalSpec Current
        {
            get
            {
                if (DateTime.Now >= _nextCheck)
                    Refresh();
                return _current;
            }
        }

        public static void Refresh()
        {
            _nextCheck = DateTime.Now + Throttle;
            try
            {
                var me = StyxWoW.Me;
                if (me == null) return;

                if (me.Level < 10)
                {
                    _current = PalSpec.Retribution;
                    return;
                }

                int tHoly = 0, tProt = 0, tRet = 0;
                for (int tab = 1; tab <= 3; tab++)
                {
                    int numTalents = Lua.GetReturnVal<int>("return GetNumTalents(" + tab + ")", 0);
                    for (int index = 1; index <= numTalents; index++)
                    {
                        // GetTalentInfo returns name,icon,tier,column,rank,... — rank is return index 4.
                        int rank = Lua.GetReturnVal<int>(
                            string.Format("return GetTalentInfo({0}, {1})", tab, index), 4);
                        if (tab == 1) tHoly += rank;
                        else if (tab == 2) tProt += rank;
                        else tRet += rank;
                    }
                }

                int max = Math.Max(tHoly, Math.Max(tProt, tRet));
                if (max == 0) { _current = PalSpec.Retribution; return; }

                if (max == tHoly) _current = PalSpec.Holy;
                else if (max == tProt) _current = PalSpec.Protection;
                else _current = PalSpec.Retribution;
            }
            catch
            {
                // Lua can briefly fail during loading screens / world changes — keep the last known spec.
            }
        }
    }
}
