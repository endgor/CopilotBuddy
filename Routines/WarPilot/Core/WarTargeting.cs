using System;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using WarPilot.Config;

namespace WarPilot.Core
{
    /// <summary>
    /// Target acquisition. Gated by the "Enable targeting" setting: when ON, fills a missing/invalid
    /// target with the nearest enemy that is attacking us; when OFF, the routine only ever acts on the
    /// target you (or the botbase) selected. It never switches off a still-valid target, so it fills
    /// gaps without hijacking botbase/manual target choice.
    /// </summary>
    public static class WarTargeting
    {
        /// <summary>Alive, attackable, selectable — the core "can I act on this unit" test.</summary>
        private static bool IsViable(WoWUnit u)
        {
            return u != null && u.Attackable && u.CanSelect && !u.Dead;
        }

        /// <summary>A viable enemy currently engaged with us or our pet.</summary>
        private static bool IsEngagedEnemy(WoWUnit u)
        {
            return IsViable(u) && u.IsTargetingMeOrPet;
        }

        public static bool HasValidTarget
        {
            get { return IsViable(StyxWoW.Me.CurrentTarget); }
        }

        // The nearby-enemy count drives the AoE decision, which is evaluated most combat ticks. A full
        // ObjectManager scan per tick is wasteful and pack size barely changes tick-to-tick, so cache the
        // result for a short window (same throttle idea as SpecDetector).
        private static float _countRange = -1f;
        private static int _countValue;
        private static DateTime _countUntil = DateTime.MinValue;
        private static readonly TimeSpan CountThrottle = TimeSpan.FromMilliseconds(300);

        /// <summary>Count of attackable enemies within <paramref name="range"/> yards engaged with us/our pet (AoE trigger).</summary>
        public static int NearbyEnemies(float range)
        {
            var now = DateTime.Now;
            if (range == _countRange && now < _countUntil)
                return _countValue;

            _countRange = range;
            _countValue = ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                .Count(u => IsEngagedEnemy(u) && u.Distance <= range);
            _countUntil = now + CountThrottle;
            return _countValue;
        }

        public static Composite CreateTargetAcquisition()
        {
            return new Decorator(
                ret => WarPilotSettings.Instance.EnableTargeting && !HasValidTarget,
                new Action(ret =>
                {
                    var target = ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                        .Where(IsEngagedEnemy)
                        .OrderBy(u => u.Distance)
                        .FirstOrDefault();

                    if (target == null)
                        return RunStatus.Failure;

                    target.Target();
                    Logging.WriteDebug("[WarPilot] acquired target " + target.Name);
                    return RunStatus.Success;
                }));
        }
    }
}
