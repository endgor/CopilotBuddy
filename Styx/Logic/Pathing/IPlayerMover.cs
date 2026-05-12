using Styx.WoWInternals;

namespace Styx.Logic.Pathing
{
    public interface IPlayerMover
    {
        void Move(WoWMovement.MovementDirection direction);

        void MoveTowards(WoWPoint location);

        void MoveStop();
    }
}
