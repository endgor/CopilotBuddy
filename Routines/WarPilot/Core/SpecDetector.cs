using System;
using Styx;
using Styx.WoWInternals;

namespace WarPilot.Core
{
    public enum WarSpec { Unknown, Arms, Fury, Protection }

    /// <summary>
    /// Determines the active warrior spec by counting spent talent points per tab via Lua
    /// (the same technique Singular's TalentManager uses). Result is cached and re-evaluated
    /// on a short throttle so a respec / dual-spec swap is picked up live without rebuilding
    /// the behavior tree (the tree branches on Current at runtime).
    ///
    /// Warrior talent tabs (3.3.5a): 1 = Arms, 2 = Fury, 3 = Protection.
    /// </summary>
    public static class SpecDetector
    {
        private static WarSpec _current = WarSpec.Unknown;
        private static DateTime _nextCheck = DateTime.MinValue;
        private static readonly TimeSpan Throttle = TimeSpan.FromSeconds(5);

        public static WarSpec Current
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

                // Below 10 there are no talents — treat as Arms (the leveling default).
                if (me.Level < 10)
                {
                    _current = WarSpec.Arms;
                    return;
                }

                int tArms = 0, tFury = 0, tProt = 0;
                for (int tab = 1; tab <= 3; tab++)
                {
                    int numTalents = Lua.GetReturnVal<int>("return GetNumTalents(" + tab + ")", 0);
                    for (int index = 1; index <= numTalents; index++)
                    {
                        // GetTalentInfo returns name,icon,tier,column,rank,... — rank is index 4.
                        int rank = Lua.GetReturnVal<int>(
                            string.Format("return GetTalentInfo({0}, {1})", tab, index), 4);
                        if (tab == 1) tArms += rank;
                        else if (tab == 2) tFury += rank;
                        else tProt += rank;
                    }
                }

                int max = Math.Max(tArms, Math.Max(tFury, tProt));
                if (max == 0) { _current = WarSpec.Arms; return; }

                if (max == tProt) _current = WarSpec.Protection;
                else if (max == tFury) _current = WarSpec.Fury;
                else _current = WarSpec.Arms;
            }
            catch
            {
                // Lua can briefly fail during loading screens / world changes — keep the last known spec.
            }
        }
    }
}
