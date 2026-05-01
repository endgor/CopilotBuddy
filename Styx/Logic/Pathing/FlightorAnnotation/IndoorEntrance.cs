// IndoorEntrance.cs — Ported from HB 6.2.3 Styx/Pathing/FlightorAnnotation/IndoorEntrance.cs
// Represents a landing waypoint at an indoor entrance for the Flightor system.
// WotLK note: annotation files are not used at runtime for 3.3.5a, but the class is
// kept as part of the ported API for completeness.

using System;
using System.Globalization;

namespace Styx.Logic.Pathing.FlightorAnnotation
{
    /// <summary>
    /// An annotated indoor-entrance location where Flightor should dismount and
    /// hand off navigation to the ground pathfinder.
    /// </summary>
    public class IndoorEntrance
    {
        /// <param name="location">World-space landing point.</param>
        /// <param name="dismount">Whether to dismount on arrival. Default true.</param>
        /// <param name="radius">Acceptance radius in yards. Default 4.</param>
        public IndoorEntrance(WoWPoint location, bool dismount = true, float radius = 4f)
        {
            Dismount = dismount;
            Location = location;
            Radius = radius;
        }

        /// <summary>Whether to dismount on arrival at this entrance.</summary>
        public bool Dismount { get; private set; }

        /// <summary>World-space location of the entrance.</summary>
        public WoWPoint Location { get; private set; }

        /// <summary>Acceptance radius in yards.</summary>
        public float Radius { get; private set; }
    }
}
