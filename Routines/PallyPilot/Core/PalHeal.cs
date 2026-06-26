using System;
using System.Collections.Generic;
using System.Linq;
using Styx.Logic.Combat;
using Styx.WoWInternals;

namespace PallyPilot.Core
{
    /// <summary>
    /// Holy-Light / Flash-of-Light rank intelligence — the engine behind "smart healing & downranking".
    ///
    /// WotLK 3.3.5a verified rank → spellId tables (low rank first). At runtime we keep only the ranks
    /// the character actually knows (SpellManager.HasSpell) and read each rank's base heal from the
    /// spell record (WoWSpell.SpellEffect1.BasePoints). To heal a deficit we pick the LOWEST known rank
    /// whose base heal already covers it — so light damage is patched with a cheap low-rank cast and big
    /// hits still pull the full-size heal. Since real heals exceed BasePoints (spell power is added on
    /// top) this never under-heals; it only trims overheal and saves mana.
    ///
    /// The known-rank list is cached and refreshed on a throttle (it only changes on level-up / training).
    /// </summary>
    public static class PalHeal
    {
        // Verified 3.3.5a spellIds, lowest rank first.
        private static readonly int[] HolyLightRanks =
            { 635, 639, 647, 1026, 1042, 3472, 10328, 10329, 25292, 27135, 27136, 48781, 48782 };

        private static readonly int[] FlashOfLightRanks =
            { 19750, 19939, 19940, 19941, 19942, 19943, 27137, 48784, 48785 };

        private struct Rank { public int Id; public int Heal; }

        private static List<Rank> _hl, _fol;
        private static DateTime _next = DateTime.MinValue;
        private static readonly TimeSpan Throttle = TimeSpan.FromSeconds(20);

        private static void EnsureCache()
        {
            if (DateTime.Now < _next && _hl != null) return;
            _next = DateTime.Now + Throttle;
            _hl = BuildKnown(HolyLightRanks);
            _fol = BuildKnown(FlashOfLightRanks);
        }

        private static List<Rank> BuildKnown(int[] ids)
        {
            var list = new List<Rank>();
            foreach (var id in ids)
            {
                if (!SpellManager.HasSpell(id)) continue;
                int heal = 0;
                try
                {
                    var spell = WoWSpell.FromId(id);
                    if (spell != null && spell.SpellEffect1 != null)
                        heal = Math.Abs(spell.SpellEffect1.BasePoints);
                }
                catch { }
                list.Add(new Rank { Id = id, Heal = heal });
            }
            return list; // already low→high
        }

        /// <summary>True if the character knows at least one rank of Holy Light.</summary>
        public static bool KnowsHolyLight { get { EnsureCache(); return _hl.Count > 0; } }

        /// <summary>True if the character knows at least one rank of Flash of Light.</summary>
        public static bool KnowsFlashOfLight { get { EnsureCache(); return _fol.Count > 0; } }

        /// <summary>Best Holy Light rank id for the given missing-health amount (0 if none known).</summary>
        public static int HolyLightId(double deficitHp, bool downrank)
        {
            EnsureCache();
            return Pick(_hl, deficitHp, downrank);
        }

        /// <summary>Best Flash of Light rank id for the given missing-health amount (0 if none known).</summary>
        public static int FlashOfLightId(double deficitHp, bool downrank)
        {
            EnsureCache();
            return Pick(_fol, deficitHp, downrank);
        }

        private static int Pick(List<Rank> ranks, double deficitHp, bool downrank)
        {
            if (ranks == null || ranks.Count == 0) return 0;
            if (!downrank) return ranks[ranks.Count - 1].Id; // top rank

            // Lowest rank whose base heal already covers the deficit; else the biggest we have.
            foreach (var r in ranks)
                if (r.Heal >= deficitHp) return r.Id;
            return ranks[ranks.Count - 1].Id;
        }
    }
}
