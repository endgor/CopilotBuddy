using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using S = PallyPilot.Config.PallyPilotSettings;

namespace PallyPilot.Core
{
    /// <summary>
    /// Targeting for both specs.
    ///   Ret  — fills a missing/invalid enemy target with the nearest mob engaged with us (gated by
    ///          "Enable targeting"); also counts nearby enemies for the AoE decision.
    ///   Holy — resolves the friendly heal target (lowest health in range / LoS) and the tank (for
    ///          Beacon of Light & Sacred Shield). Group scans are cached on a short throttle so we
    ///          don't walk the ObjectManager every tick.
    /// </summary>
    public static class PalTargeting
    {
        private static S Cfg { get { return S.Instance; } }

        // ===================== Retribution (enemy) =====================

        private static bool IsViable(WoWUnit u)
        {
            return u != null && u.Attackable && u.CanSelect && !u.Dead;
        }

        private static bool IsEngagedEnemy(WoWUnit u)
        {
            return IsViable(u) && u.IsTargetingMeOrPet;
        }

        public static bool HasValidTarget { get { return IsViable(StyxWoW.Me.CurrentTarget); } }

        private static float _countRange = -1f;
        private static int _countValue;
        private static DateTime _countUntil = DateTime.MinValue;
        private static readonly TimeSpan CountThrottle = TimeSpan.FromMilliseconds(300);

        /// <summary>Count of attackable enemies within range engaged with us/our pet (AoE trigger).</summary>
        public static int NearbyEnemies(float range)
        {
            var now = DateTime.Now;
            if (range == _countRange && now < _countUntil) return _countValue;
            _countRange = range;
            _countValue = ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                .Count(u => IsEngagedEnemy(u) && u.Distance <= range);
            _countUntil = now + CountThrottle;
            return _countValue;
        }

        public static Composite CreateTargetAcquisition()
        {
            return new Decorator(
                ret => Cfg.EnableTargeting && !HasValidTarget,
                new Action(ret =>
                {
                    var target = ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                        .Where(IsEngagedEnemy)
                        .OrderBy(u => u.Distance)
                        .FirstOrDefault();
                    if (target == null) return RunStatus.Failure;
                    target.Target();
                    Logging.WriteDebug("[PallyPilot] acquired target " + target.SafeName());
                    return RunStatus.Success;
                }));
        }

        // ===================== Holy (friendly) =====================

        private static List<WoWUnit> _friends;
        private static DateTime _friendsUntil = DateTime.MinValue;
        private static readonly TimeSpan FriendThrottle = TimeSpan.FromMilliseconds(300);

        /// <summary>Heal candidates: self + group members that are alive, in range and in line of sight.</summary>
        public static List<WoWUnit> FriendlyUnits()
        {
            var now = DateTime.Now;
            if (_friends != null && now < _friendsUntil) return _friends;

            float range = Cfg.HolyHealRange;
            var seen = new HashSet<ulong>();
            var list = new List<WoWUnit>();

            var me = StyxWoW.Me;
            if (me != null && !me.Dead) { list.Add(me); seen.Add(me.Guid); }

            try
            {
                foreach (var pm in me.GroupInfo.AllMembers)
                {
                    var p = pm.ToPlayer();
                    if (p == null || p.Dead) continue;
                    if (!seen.Add(p.Guid)) continue;
                    if (p.Distance > range || !p.InLineOfSight) continue;
                    list.Add(p);
                }
            }
            catch { }

            _friends = list;
            _friendsUntil = now + FriendThrottle;
            return _friends;
        }

        /// <summary>The most-hurt heal candidate (lowest health %). Null when nobody qualifies.</summary>
        public static WoWUnit LowestHealth()
        {
            WoWUnit low = null;
            double lowPct = 999;
            foreach (var u in FriendlyUnits())
            {
                double hp = u.HealthPercent;
                if (hp < lowPct) { lowPct = hp; low = u; }
            }
            return low;
        }

        /// <summary>The lowest-health candidate that is below <paramref name="pct"/>; null if everyone is healthy.</summary>
        public static WoWUnit LowestHealthBelow(double pct)
        {
            var low = LowestHealth();
            return (low != null && low.HealthPercent < pct) ? low : null;
        }

        private static WoWUnit _tank;
        private static DateTime _tankUntil = DateTime.MinValue;
        private static readonly TimeSpan TankThrottle = TimeSpan.FromSeconds(2);

        /// <summary>
        /// The tank to anchor Beacon of Light / Sacred Shield on: the group's flagged tank if present,
        /// otherwise (when "Beacon self if no tank" is on) the paladin itself for solo/leveling Holy.
        /// </summary>
        public static WoWUnit Tank()
        {
            var now = DateTime.Now;
            if (now < _tankUntil) return _tank;
            _tankUntil = now + TankThrottle;
            _tank = null;

            var me = StyxWoW.Me;
            try
            {
                var pm = me.GroupInfo.Tanks.FirstOrDefault();
                if (pm != null)
                {
                    var p = pm.ToPlayer();
                    if (p != null && !p.Dead) _tank = p;
                }
            }
            catch { }

            if (_tank == null && Cfg.HolyBeaconSelfIfSolo)
                _tank = me;
            return _tank;
        }
    }
}
