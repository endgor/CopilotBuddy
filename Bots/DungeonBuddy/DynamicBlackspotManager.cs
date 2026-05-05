using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Tripper.Navigation;
using PathNavigator = Styx.Logic.Pathing.Navigator;

namespace Bots.DungeonBuddy
{
    /// <summary>
    /// HB WoD DungeonBuddy dynamic blackspot manager. Restores polygon areas when blackspots expire.
    /// </summary>
    public static class DynamicBlackspotManager
    {
        private static readonly List<DynamicBlackspot> _blackspots = new List<DynamicBlackspot>();
        private static readonly Dictionary<PolygonReference, (uint MapId, byte Area)> _originalAreas = new Dictionary<PolygonReference, (uint MapId, byte Area)>();

        static DynamicBlackspotManager()
        {
            PathNavigator.TripperNavigator.TileLoaded += OnTileLoaded;
        }

        public static void Pulse()
        {
            bool changed = false;

            foreach (DynamicBlackspot blackspot in _blackspots.Where(blackspot => blackspot.ShouldRemove && blackspot.ClearApplied()).ToArray())
            {
                if (!string.IsNullOrEmpty(blackspot.Name))
                    Logging.Write(Colors.RoyalBlue, "Removed blackspot for {0}", blackspot.Name);

                changed = true;
            }

            if (changed)
                Reset();

            foreach (DynamicBlackspot blackspot in _blackspots.Where(blackspot => blackspot.ShouldApplyNow && blackspot.Apply()).ToArray())
            {
                if (blackspot.EffectedPolys == null)
                    continue;

                lock (_originalAreas)
                {
                    foreach (KeyValuePair<PolygonReference, byte> polygonArea in blackspot.EffectedPolys)
                    {
                        if (!_originalAreas.ContainsKey(polygonArea.Key))
                            _originalAreas[polygonArea.Key] = ((uint)blackspot.MapId, polygonArea.Value);
                    }
                }

                if (!string.IsNullOrEmpty(blackspot.Name))
                    Logging.Write(Colors.RoyalBlue, "Applied blackspot for {0}", blackspot.Name);

                changed = true;
            }

            if (changed)
                PathNavigator.Clear();
        }

        public static void AddBlackspot(DynamicBlackspot blackspot)
        {
            _blackspots.Add(blackspot);
        }

        public static void AddBlackspots(params DynamicBlackspot[] blackspots)
        {
            AddBlackspots((IEnumerable<DynamicBlackspot>)blackspots);
        }

        public static void AddBlackspots(IEnumerable<DynamicBlackspot> blackspots)
        {
            foreach (DynamicBlackspot blackspot in blackspots)
                _blackspots.Add(blackspot);
        }

        public static void RemoveBlackspot(DynamicBlackspot blackspot)
        {
            RemoveBlackspots(new[] { blackspot });
        }

        public static void RemoveBlackspots(params DynamicBlackspot[] blackspots)
        {
            RemoveBlackspots((IEnumerable<DynamicBlackspot>)blackspots);
        }

        public static void RemoveBlackspots(IEnumerable<DynamicBlackspot> blackspots)
        {
            Reset();
            foreach (DynamicBlackspot blackspot in blackspots)
                _blackspots.Remove(blackspot);
        }

        public static void Clear()
        {
            Reset();
            _blackspots.Clear();
        }

        public static void Reset()
        {
            foreach (DynamicBlackspot blackspot in _blackspots)
                blackspot.ClearApplied();

            var nav = PathNavigator.TripperNavigator;
            lock (_originalAreas)
            {
                foreach (KeyValuePair<PolygonReference, (uint MapId, byte Area)> polygonArea in _originalAreas)
                    nav.SetPolyArea(polygonArea.Value.MapId, polygonArea.Key, polygonArea.Value.Area);

                _originalAreas.Clear();
            }
        }

        private static void OnTileLoaded(object? sender, TileLoadedEventArgs e)
        {
            Reset();
        }
    }
}
