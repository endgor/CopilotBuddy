using System;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Pathing;
using TreeSharp;
using Action = TreeSharp.Action;
using WarPilot.Config;

namespace WarPilot.Core
{
    /// <summary>
    /// Minimal self-contained movement helpers (face / move-to-melee). DLL-native only.
    /// Per project rule, the rotation places these LAST so spells are tried before moving.
    /// </summary>
    public static class WarMovement
    {
        /// <summary>Effective melee range to the current target (mirrors Singular's calc).</summary>
        public static float MeleeRange
        {
            get
            {
                var me = StyxWoW.Me;
                var t = me.CurrentTarget;
                if (t == null) return 5f;
                if (t.IsPlayer) return 3.5f;
                return Math.Max(5f, me.CombatReach + 1.3333334f + t.CombatReach);
            }
        }

        // Charge usable range (yards). One home for the literals shared by every spec's pull.
        public const float ChargeMin = 8f;
        public const float ChargeMax = 25f;

        // Single chokepoint for the "Enable movement" setting so every mover (move-to-melee, Charge,
        // and any future gap-closer) respects it consistently — a new mover can't silently escape the gate.
        private static bool MovementEnabled { get { return WarPilotSettings.Instance.EnableMovement; } }

        /// <summary>Charge the current target when within range. Gated by Enable movement (Charge relocates you).</summary>
        public static Composite CreateCharge()
        {
            return WarSpell.Cast("Charge",
                ret => MovementEnabled
                       && StyxWoW.Me.GotTarget
                       && StyxWoW.Me.CurrentTarget.Distance >= ChargeMin
                       && StyxWoW.Me.CurrentTarget.Distance <= ChargeMax);
        }

        /// <summary>Face the current target if we are not already roughly facing it.</summary>
        public static Composite CreateFaceTarget()
        {
            return new Decorator(
                ret => StyxWoW.Me.GotTarget && !StyxWoW.Me.IsSafelyFacing(StyxWoW.Me.CurrentTarget, 70f),
                new Action(ret =>
                {
                    WoWMovement.Face(StyxWoW.Me.CurrentTarget.Guid);
                    return RunStatus.Success;
                }));
        }

        /// <summary>
        /// Move into melee of the current target; stop on arrival. The whole behavior is gated on
        /// the "Enable movement" setting — when off, the routine never moves OR stops the character,
        /// so the user keeps full manual control of positioning.
        /// </summary>
        public static Composite CreateMoveToMelee()
        {
            return new Decorator(
                ret => MovementEnabled,
                new PrioritySelector(
                    new Decorator(
                        ret => StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.Distance > MeleeRange,
                        new Action(ret =>
                        {
                            Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                            return RunStatus.Running;
                        })),
                    new Decorator(
                        ret => StyxWoW.Me.IsMoving,
                        new Action(ret =>
                        {
                            Navigator.PlayerMover.MoveStop();
                            return RunStatus.Failure;
                        }))));
        }
    }
}
