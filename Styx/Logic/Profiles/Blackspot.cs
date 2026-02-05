#nullable disable
using System;
using Styx.Logic.Pathing;

namespace Styx.Logic.Profiles
{
    /// <summary>
    /// Represents a blackspot - an area to avoid during navigation.
    /// </summary>
    public struct Blackspot : IEquatable<Blackspot>
    {
        /// <summary>
        /// Gets or sets the center location of the blackspot.
        /// </summary>
        public WoWPoint Location;

        /// <summary>
        /// Gets or sets the radius of the blackspot.
        /// </summary>
        public float Radius;

        /// <summary>
        /// Gets or sets the height of the blackspot cylinder.
        /// </summary>
        public float Height;

        /// <summary>
        /// Initializes a new blackspot.
        /// </summary>
        /// <param name="location">The center location.</param>
        /// <param name="radius">The radius to avoid.</param>
        /// <param name="height">The height of the area.</param>
        public Blackspot(WoWPoint location, float radius, float height)
        {
            Location = location;
            Radius = radius;
            Height = height;
        }

        /// <summary>
        /// Checks if a point is within this blackspot.
        /// </summary>
        public bool Contains(WoWPoint point)
        {
            float dx = point.X - Location.X;
            float dy = point.Y - Location.Y;
            float dz = point.Z - Location.Z;

            // Check horizontal distance
            if (dx * dx + dy * dy > Radius * Radius)
                return false;

            // Check vertical distance
            return dz >= 0 && dz <= Height;
        }

        public override string ToString()
        {
            return $"[Blackspot Location: {Location}, Radius: {Radius}, Height: {Height}]";
        }

        public bool Equals(Blackspot other)
        {
            return Location.Equals(other.Location) && 
                   Math.Abs(Radius - other.Radius) < 0.001f && 
                   Math.Abs(Height - other.Height) < 0.001f;
        }

        public override bool Equals(object obj)
        {
            return obj is Blackspot other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location, Radius, Height);
        }

        public static bool operator ==(Blackspot left, Blackspot right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Blackspot left, Blackspot right)
        {
            return !left.Equals(right);
        }
    }
}
