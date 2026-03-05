using System;
using System.Collections.Generic;
using System.Linq;
using Styx.Helpers;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#nullable disable
namespace Styx.Logic.Pathing
{
    /// <summary>
    /// Manages dynamic avoidance of hostile mobs by creating temporary blackspots.
    /// Mobs can be added to avoid by Entry ID, and the system will automatically
    /// create and update blackspots as mobs move.
    /// </summary>
    public static class AvoidanceManager
    {
        private static readonly HashSet<uint> _avoidEntries = new HashSet<uint>();
        private static readonly Dictionary<ulong, AvoidanceData> _trackedMobs = new Dictionary<ulong, AvoidanceData>();
        private static readonly object _lock = new object();

        /// <summary>
        /// Distance threshold for tracking mobs (60 yards squared = 3600).
        /// </summary>
        private const float TrackingDistanceSqr = 3600f;

        /// <summary>
        /// Distance threshold for updating mob position (10 yards squared = 100).
        /// </summary>
        private const float MoveThresholdSqr = 100f;

        /// <summary>
        /// Height of avoidance blackspots.
        /// </summary>
        private const float AvoidanceHeight = 10f;

        /// <summary>
        /// Extra padding added to aggro range for avoidance.
        /// </summary>
        private const float AvoidancePadding = 10f;

        static AvoidanceManager()
        {
            BotEvents.Profile.OnNewProfileLoaded += OnNewProfileLoaded;
        }

        private static void OnNewProfileLoaded(BotEvents.Profile.NewProfileLoadedEventArgs args)
        {
            // Remove avoidance from old profile
            if (args.OldProfile?.AvoidMobs?.HashSet1 != null)
            {
                RemoveAll(args.OldProfile.AvoidMobs.HashSet1);
                Navigator.Clear();
            }

            // Add avoidance from new profile
            if (args.NewProfile?.AvoidMobs?.HashSet1 != null)
            {
                AddAll(args.NewProfile.AvoidMobs.HashSet1);
                Navigator.Clear();
            }
        }

        /// <summary>
        /// Updates avoidance blackspots based on current mob positions.
        /// Should be called periodically (e.g., in the bot's pulse).
        /// </summary>
        public static void Pulse()
        {
            if (StyxWoW.Me == null)
                return;

            WoWPoint playerLocation = StyxWoW.Me.Location;

            lock (_lock)
            {
                var units = ObjectManager.CachedUnits;
                if (units == null)
                    return;

                foreach (var unit in units)
                {
                    if (!_avoidEntries.Contains(unit.Entry))
                        continue;

                    if (unit.IsFriendly)
                        continue;

                    WoWPoint mobLocation = unit.Location;
                    float distanceSqr = playerLocation.DistanceSqr(mobLocation);

                    if (_trackedMobs.TryGetValue(unit.Guid, out var data))
                    {
                        // Mob is tracked - check if it's now too far away
                        if (distanceSqr > TrackingDistanceSqr)
                        {
                            // Remove tracking - mob is too far
                            _trackedMobs.Remove(unit.Guid);
                            BlackspotManager.RemoveBlackspot(data.Blackspot);
                            Logging.WriteDebug($"Removing '{unit.Name}' as a danger - not avoiding him anymore!");
                        }
                        else if (mobLocation.DistanceSqr(data.LastPosition) > MoveThresholdSqr)
                        {
                            // Mob has moved significantly - update blackspot
                            BlackspotManager.RemoveBlackspot(data.Blackspot);
                            Navigator.Clear();

                            float aggroRange = GetAggroRange(unit);
                            var newBlackspot = new Blackspot(mobLocation, aggroRange + AvoidancePadding, AvoidanceHeight);

                            _trackedMobs[unit.Guid] = new AvoidanceData(unit.Entry, mobLocation, newBlackspot);
                            BlackspotManager.AddBlackspot(mobLocation, aggroRange + AvoidancePadding, AvoidanceHeight);

                            Logging.WriteDebug($"'{unit.Name}' is walking! Reavoiding!");
                        }
                    }
                    else if (distanceSqr < TrackingDistanceSqr)
                    {
                        // New mob to track
                        float aggroRange = GetAggroRange(unit);
                        var blackspot = new Blackspot(mobLocation, aggroRange + AvoidancePadding, AvoidanceHeight);

                        _trackedMobs.Add(unit.Guid, new AvoidanceData(unit.Entry, mobLocation, blackspot));
                        BlackspotManager.AddBlackspot(mobLocation, aggroRange + AvoidancePadding, AvoidanceHeight);
                        Navigator.Clear();

                        Logging.WriteDebug($"Avoiding '{unit.Name}'");
                    }
                }
            }
        }

        private static float GetAggroRange(WoWUnit unit)
        {
            // Default aggro range based on level difference
            if (StyxWoW.Me == null)
                return 20f;

            int levelDiff = unit.Level - StyxWoW.Me.Level;
            float baseRange = 20f;

            if (levelDiff > 0)
                baseRange += levelDiff * 1f;
            else if (levelDiff < -5)
                baseRange -= 5f;

            return Math.Max(5f, Math.Min(45f, baseRange));
        }

        /// <summary>
        /// Adds a mob entry ID to the avoidance list.
        /// </summary>
        public static void Add(uint entryId)
        {
            lock (_lock)
            {
                _avoidEntries.Add(entryId);
            }
        }

        /// <summary>
        /// Adds multiple mob entry IDs to the avoidance list.
        /// </summary>
        public static void AddAll(IEnumerable<uint> entryIds)
        {
            if (entryIds == null)
                return;

            lock (_lock)
            {
                foreach (uint id in entryIds)
                {
                    _avoidEntries.Add(id);
                }
            }
        }

        /// <summary>
        /// Removes a mob entry ID from the avoidance list.
        /// </summary>
        public static bool Remove(uint entryId)
        {
            lock (_lock)
            {
                if (!_avoidEntries.Remove(entryId))
                    return false;

                // Remove all tracked mobs with this entry
                var toRemove = _trackedMobs
                    .Where(kvp => kvp.Value.EntryId == entryId)
                    .ToList();

                foreach (var kvp in toRemove)
                {
                    BlackspotManager.RemoveBlackspot(kvp.Value.Blackspot);
                    _trackedMobs.Remove(kvp.Key);
                }

                return true;
            }
        }

        /// <summary>
        /// Removes multiple mob entry IDs from the avoidance list.
        /// </summary>
        public static void RemoveAll(IEnumerable<uint> entryIds)
        {
            if (entryIds == null)
                return;

            var idsToRemove = new HashSet<uint>(entryIds);

            lock (_lock)
            {
                // Remove blackspots for tracked mobs
                var toRemove = _trackedMobs
                    .Where(kvp => idsToRemove.Contains(kvp.Value.EntryId))
                    .ToList();

                foreach (var kvp in toRemove)
                {
                    BlackspotManager.RemoveBlackspot(kvp.Value.Blackspot);
                    _trackedMobs.Remove(kvp.Key);
                }

                // Remove from avoid entries
                _avoidEntries.RemoveWhere(idsToRemove.Contains);
            }
        }

        /// <summary>
        /// Clears all avoidance data.
        /// </summary>
        public static void Clear()
        {
            lock (_lock)
            {
                foreach (var data in _trackedMobs.Values)
                {
                    BlackspotManager.RemoveBlackspot(data.Blackspot);
                }

                _trackedMobs.Clear();
                _avoidEntries.Clear();
            }
        }

        /// <summary>
        /// Checks if a mob entry ID is being avoided.
        /// </summary>
        public static bool IsAvoiding(uint entryId)
        {
            lock (_lock)
            {
                return _avoidEntries.Contains(entryId);
            }
        }

        /// <summary>
        /// Internal data for tracking avoided mobs.
        /// </summary>
        private readonly struct AvoidanceData
        {
            public readonly uint EntryId;
            public readonly WoWPoint LastPosition;
            public readonly Blackspot Blackspot;

            public AvoidanceData(uint entryId, WoWPoint position, Blackspot blackspot)
            {
                EntryId = entryId;
                LastPosition = position;
                Blackspot = blackspot;
            }
        }
    }
}
