using Styx.Logic.Pathing;

namespace Bots.DungeonBuddy.Profiles.Handlers
{
    public struct Blackspot
    {
        public WoWPoint Location => new WoWPoint(X, Y, Z);

        public static implicit operator Styx.Logic.Profiles.Blackspot(Blackspot bs)
        {
            return new Styx.Logic.Profiles.Blackspot(bs.Location, bs.Radius, bs.Height);
        }

        public float X;
        public float Y;
        public float Z;
        public float Radius;
        public float Height;
    }
}