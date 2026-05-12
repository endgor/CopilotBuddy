using System;
using Styx.Helpers;
using Styx.WoWInternals;

namespace Styx.Logic.Pathing
{
    /// <summary>
    /// WoD-style keyboard mover — port of HB 6.2.3 Styx.Pathing.KeyboardMover.
    /// Steers by calling SetFacing in a tight loop rather than CTM.
    /// </summary>
    public class KeyboardMover : IPlayerMover
    {
        // HB 6.2.3 KeyboardMover.float_0: last non-zero rotation used for angle delta
        private float _lastRotation;

        public void Move(WoWMovement.MovementDirection direction)
        {
            WoWMovement.Move(direction);
        }

        // HB 6.2.3 KeyboardMover.method_0: compute signed angle delta from current facing to target.
        private float CalculateAngleDelta(WoWPoint target)
        {
            float neededFacing = WoWMathHelper.CalculateNeededFacing(ObjectManager.Me.Location, target);
            float rotation = ObjectManager.Me.Rotation;
            _lastRotation = (rotation != 0f) ? rotation : _lastRotation;

            const float pi = 3.14159274f;
            float delta = neededFacing - _lastRotation;
            if (delta > pi)
                delta = -(pi - (delta - pi));
            else if (delta < -pi)
                delta = pi - (-pi - delta);
            return delta;
        }

        // HB 6.2.3 KeyboardMover.MoveTowards: keyboard steering — adjusts facing then moves forward.
        public void MoveTowards(WoWPoint location)
        {
            if (WoWMathHelper.IsFacing(ObjectManager.Me.Location, ObjectManager.Me.Rotation, location, 2f))
            {
                WoWMovement.Move(WoWMovement.MovementDirection.Forward);
            }
            else
            {
                WoWMovement.MoveStop();
            }

            float delta = CalculateAngleDelta(location);
            float step = Math.Abs(delta) / 6f;
            if (step < 0.1f)
                step = Math.Abs(delta);
            float threshold = step + 0.05f;

            if (delta < 0f)
            {
                bool adjusting = true;
                while (adjusting)
                {
                    ObjectManager.Me.SetFacing(ObjectManager.Me.Rotation - step);
                    delta = CalculateAngleDelta(location);
                    if ((double)(threshold - delta) < (double)threshold + 0.1 || (delta > 0f && delta > threshold))
                        adjusting = false;
                }
                return;
            }

            if (delta > 0f)
            {
                bool adjusting = true;
                while (adjusting)
                {
                    ObjectManager.Me.SetFacing(ObjectManager.Me.Rotation + step);
                    delta = CalculateAngleDelta(location);
                    if ((double)(delta - threshold) < 0.1 || (delta < 0f && delta < -threshold))
                        adjusting = false;
                }
            }
        }

        public void MoveStop()
        {
            WoWMovement.MoveStop();
        }
    }

    /// <summary>
    /// Default WoD-style click-to-move mover.
    /// </summary>
    public class ClickToMoveMover : IPlayerMover
    {
        public void Move(WoWMovement.MovementDirection direction)
        {
            WoWMovement.Move(direction);
        }

        public void MoveTowards(WoWPoint location)
        {
            WoWMovement.ClickToMove(location);
        }

        public void MoveStop()
        {
            WoWMovement.MoveStop();
        }
    }
}
