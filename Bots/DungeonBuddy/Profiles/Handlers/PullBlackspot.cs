using Styx.Logic.Pathing;

namespace Bots.DungeonBuddy.Profiles.Handlers
{
    public struct PullBlackspot
    {
        public WoWPoint Location => new WoWPoint(X, Y, Z);

        public float X;
        public float Y;
        public float Z;
        public float Radius;
        public float Height;
    }
}