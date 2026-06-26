using System;
using System.Linq;
using Styx;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PallyPilot.Core
{
    /// <summary>
    /// Protection (tank) helpers: loose-mob / taunt detection, crowd-control awareness, and
    /// defensive-cooldown bookkeeping. The paladin twin of WarPilot's WarTank — same transferable
    /// patterns (CC-aware AoE so we never break a sheep/sap/trap, one-major-defensive-at-a-time,
    /// "only act while actually being attacked"), reimplemented for WotLK 3.3.5a paladin abilities.
    ///
    /// Taunt model differs from the warrior: the paladin's single-target taunt is Hand of Reckoning
    /// (30 yd ranged), and the AoE taunt is Righteous Defense, which is cast on a FRIENDLY and pulls up
    /// to 3 mobs attacking that ally onto us. So this exposes both the loose enemy (for Hand of
    /// Reckoning) and the beleaguered friendly (for Righteous Defense).
    /// </summary>
    public static class PalTank
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // "Soft" CC an AoE would break — sheep, sap, banish, shackle, sleep, incapacitate, fear/horror.
        // Deliberately EXCLUDES Stunned/Rooted: those are short or ours. Mirrors WarTank.CcMechanics.
        private static readonly WoWSpellMechanic[] CcMechanics =
        {
            WoWSpellMechanic.Polymorphed, WoWSpellMechanic.Sapped, WoWSpellMechanic.Banished,
            WoWSpellMechanic.Shackled, WoWSpellMechanic.Asleep, WoWSpellMechanic.Incapacitated,
            WoWSpellMechanic.Horrified
        };

        // --- shared cached scan (one ObjectManager pass per throttle window for every tank check) ---
        private static float _scanRange = -1f;
        private static DateTime _scanUntil = DateTime.MinValue;
        private static WoWUnit[] _scan = new WoWUnit[0];
        private static readonly TimeSpan ScanThrottle = TimeSpan.FromMilliseconds(300);

        private static WoWUnit[] NearbyUnits(float range)
        {
            var now = DateTime.Now;
            if (range == _scanRange && now < _scanUntil) return _scan;
            _scanRange = range;
            _scan = ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                .Where(u => u != null && u.IsValid && !u.Dead && u.Distance <= range)
                .ToArray();
            _scanUntil = now + ScanThrottle;
            return _scan;
        }

        /// <summary>
        /// True if an AoE within <paramref name="range"/> would hit a crowd-controlled enemy that is NOT
        /// our current target — i.e. it would break a teammate's sheep/sap/trap. Skip the AoE if so.
        /// </summary>
        public static bool WouldBreakCC(float range)
        {
            var target = Me.CurrentTarget;
            foreach (var u in NearbyUnits(range))
            {
                if (target != null && u.Guid == target.Guid) continue;
                if (!u.Attackable) continue;
                foreach (var m in CcMechanics)
                    if (u.HasAuraWithMechanic(m)) return true;
            }
            return false;
        }

        /// <summary>A loose mob is attacking a groupmate (not me/my pet) — a taunt candidate.</summary>
        private static bool IsLoose(WoWUnit u)
        {
            return u != null && u.Attackable && u.CanSelect && !u.Dead
                   && !u.IsTargetingMeOrPet
                   && (u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember);
        }

        /// <summary>
        /// Nearest loose mob within <paramref name="range"/> to Hand of Reckoning (single-target taunt),
        /// or null. Hand of Reckoning is a 30 yd ranged taunt, so this reaches further than a melee taunt.
        /// </summary>
        public static WoWUnit FindTauntTarget(float range)
        {
            return NearbyUnits(range).Where(IsLoose).OrderBy(u => u.Distance).FirstOrDefault();
        }

        /// <summary>Count of loose mobs within <paramref name="range"/> (drives the Righteous Defense decision).</summary>
        public static int LooseMobCount(float range)
        {
            return NearbyUnits(range).Count(IsLoose);
        }

        /// <summary>
        /// The friendly ally (party/raid member) with the most loose mobs on it — the unit to aim
        /// Righteous Defense at (it taunts up to 3 of the mobs attacking that friendly onto us). Returns
        /// null when no ally has at least <paramref name="minAttackers"/> loose mobs within
        /// <paramref name="range"/>. Never returns the paladin itself (a mob on us isn't "loose").
        /// </summary>
        public static WoWUnit RighteousDefenseTarget(float range, int minAttackers)
        {
            var loose = NearbyUnits(range).Where(IsLoose).ToArray();
            if (loose.Length == 0) return null;

            // Group loose mobs by the friendly they are pounding, then pick the most-attacked ally.
            WoWUnit bestFriend = null;
            int best = 0;
            foreach (var grp in loose.Where(u => u.CurrentTarget != null)
                                     .GroupBy(u => u.CurrentTarget.Guid))
            {
                int n = grp.Count();
                if (n <= best) continue;
                var friend = grp.First().CurrentTarget;
                if (friend == null || friend.Dead || !friend.IsValid) continue;
                if (!(friend.IsPlayer || friend.IsPet)) continue;     // friendlies only
                best = n;
                bestFriend = friend;
            }
            return best >= minAttackers ? bestFriend : null;
        }

        /// <summary>A major self-mitigation cooldown is already running — don't stack a second one.</summary>
        public static bool MajorDefensiveActive
        {
            get
            {
                return Me.HasAura("Divine Protection") || Me.HasAura("Divine Shield")
                       || Me.HasAura("Divine Sacrifice");
            }
        }

        /// <summary>True while something is actually hitting us (gate reactive mitigation on real aggro).</summary>
        public static bool BeingAttacked
        {
            get { return Me.Combat && NearbyUnits(40f).Any(u => u.IsTargetingMeOrPet); }
        }
    }
}
