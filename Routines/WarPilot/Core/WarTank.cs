using System;
using System.Linq;
using Styx;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace WarPilot.Core
{
    /// <summary>
    /// Protection (tank) helpers: loose-mob/taunt detection, crowd-control awareness, and
    /// defensive-cooldown bookkeeping. These encode the transferable patterns read out of TuanHA's
    /// (Legion) Prot rotation — CC-aware AoE (don't break a sheep/sap/trap), one-defensive-at-a-time,
    /// and "only act while actually being attacked" — reimplemented clean for WotLK 3.3.5a.
    /// </summary>
    public static class WarTank
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // "Soft" CC an AoE would break — sheep, sap, trap, hibernate, shackle, banish, repentance.
        // Deliberately EXCLUDES Stunned/Rooted/Frozen-by-us: those are short or ours to break.
        private static readonly WoWSpellMechanic[] CcMechanics =
        {
            WoWSpellMechanic.Polymorphed, WoWSpellMechanic.Sapped, WoWSpellMechanic.Banished,
            WoWSpellMechanic.Shackled, WoWSpellMechanic.Asleep, WoWSpellMechanic.Incapacitated,
            WoWSpellMechanic.Horrified
        };

        // --- shared cached scan (one ObjectManager pass for both CC + loose-mob checks) ---
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

        /// <summary>Nearest loose mob within <paramref name="range"/> to single-target Taunt, or null.</summary>
        public static WoWUnit FindTauntTarget(float range)
        {
            return NearbyUnits(range).Where(IsLoose).OrderBy(u => u.Distance).FirstOrDefault();
        }

        /// <summary>Count of loose mobs within <paramref name="range"/> (drives Challenging Shout).</summary>
        public static int LooseMobCount(float range)
        {
            return NearbyUnits(range).Count(IsLoose);
        }

        /// <summary>A major mitigation cooldown is already running — don't stack a second one.</summary>
        public static bool MajorDefensiveActive
        {
            get
            {
                return Me.HasAura("Shield Wall") || Me.HasAura("Last Stand") || Me.HasAura("Enraged Regeneration");
            }
        }

        /// <summary>True while something is actually hitting us (gate threat/mitigation on real aggro).</summary>
        public static bool BeingAttacked
        {
            get { return Me.Combat && NearbyUnits(40f).Any(u => u.IsTargetingMeOrPet); }
        }
    }
}
