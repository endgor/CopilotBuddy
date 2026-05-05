using System;
using System.Collections.Generic;
using System.Numerics;
using Styx;
using Styx.Logic.Pathing;
using Tripper.Navigation;
using PathNavigator = Styx.Logic.Pathing.Navigator;

namespace Bots.DungeonBuddy
{
    /// <summary>
    /// HB WoD DungeonBuddy dynamic blackspot: marks nearby navmesh polygons while a condition is active.
    /// </summary>
    public class DynamicBlackspot
    {
        private readonly Func<bool> _shouldApply;
        private bool _isApplied;

        public DynamicBlackspot(Func<bool> shouldApply, Func<WoWPoint> locationSelector, int mapId, float radius, float height = 10f, string? name = null, bool isBlocking = false)
        {
            _shouldApply = shouldApply ?? throw new ArgumentNullException(nameof(shouldApply));
            LocationSelector = locationSelector ?? throw new ArgumentNullException(nameof(locationSelector));
            Radius = radius;
            Height = height;
            Name = name;
            MapId = mapId;
            IsBlocking = isBlocking;
        }

        public Dictionary<PolygonReference, byte>? EffectedPolys { get; private set; }

        public WoWPoint Location { get; private set; }

        public readonly float Height;
        public readonly Func<WoWPoint> LocationSelector;
        public readonly int MapId;
        public readonly string? Name;
        public readonly float Radius;
        public readonly bool IsBlocking;

        internal bool ShouldApplyNow
        {
            get
            {
                return (!_isApplied || LocationSelector().DistanceSqr(Location) > 49f)
                    && StyxWoW.Me.MapId == (uint)MapId
                    && _shouldApply();
            }
        }

        internal bool ShouldRemove
        {
            get
            {
                return _isApplied && (!_shouldApply() || StyxWoW.Me.MapId != (uint)MapId);
            }
        }

        internal bool Apply()
        {
            if (_isApplied)
                return false;

            var nav = PathNavigator.TripperNavigator;
            Location = LocationSelector();

            var center = new Vector3(Location.X, Location.Y, Location.Z);
            var extents = new Vector3(Radius, Radius, Height);
            PolygonReference[] polygons = nav.QueryPolygons((uint)MapId, center, extents, 8192);

            if (polygons.Length == 0)
                return false;

            EffectedPolys = new Dictionary<PolygonReference, byte>();
            foreach (PolygonReference polygon in polygons)
            {
                uint getAreaStatus = nav.GetPolyArea((uint)MapId, polygon, out byte originalArea);
                if (new Status(getAreaStatus).Failed)
                    continue;

                EffectedPolys[polygon] = originalArea;
                nav.SetPolyArea((uint)MapId, polygon, (byte)(IsBlocking ? AreaType.Blocked : AreaType.Blackspot));
            }

            _isApplied = true;
            return true;
        }

        internal bool ClearApplied()
        {
            if (!_isApplied)
                return false;

            EffectedPolys = null;
            _isApplied = false;
            return true;
        }
    }
}
