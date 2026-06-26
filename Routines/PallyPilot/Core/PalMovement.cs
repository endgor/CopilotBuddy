using System;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Pathing;
using TreeSharp;
using Action = TreeSharp.Action;
using S = PallyPilot.Config.PallyPilotSettings;

namespace PallyPilot.Core
{
    /// <summary>
    /// Minimal self-contained movement helpers (face / move-to-melee). DLL-native only. Per project
    /// rule the Ret rotation places these LAST so spells are tried before moving. All movement honours
    /// the "Enable movement" setting so the user can keep manual control of positioning.
    /// </summary>
    public static class PalMovement
    {
        private static bool MovementEnabled { get { return S.Instance.EnableMovement; } }

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

        /// <summary>Move into melee of the current target; stop on arrival. Whole behavior gated on Enable movement.</summary>
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

        /// <summary>Move into casting range of a friendly heal target that is too far to heal. Gated on Enable movement.</summary>
        public static Composite CreateMoveToHealTarget(UnitDelegate onUnit, float range)
        {
            return new Decorator(
                ret => MovementEnabled,
                new Decorator(
                    ret =>
                    {
                        var u = onUnit(ret);
                        return u != null && (u.Distance > range || !u.InLineOfSight);
                    },
                    new Action(ret =>
                    {
                        var u = onUnit(ret);
                        if (u == null) return RunStatus.Failure;
                        Navigator.MoveTo(u.Location);
                        return RunStatus.Running;
                    })));
        }
    }
}
