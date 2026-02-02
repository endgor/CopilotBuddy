#nullable disable
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Styx.Helpers;
using Styx.Logic.Pathing;

namespace Styx.Database
{
    /// <summary>
    /// Provides creature spawn lookups from CreatureSpawns.db.
    /// Used to auto-generate hotspots when profile doesn't define them.
    /// </summary>
    public static class CreatureSpawnQueries
    {
        private static SQLiteConnection _connection;
        private static bool _initialized = false;
        private static bool _isAvailable = false;

        // Cached SQL commands
        private static SQLiteCommand _getSpawnsByEntryCmd;
        private static SQLiteCommand _getSpawnsByEntryAndMapCmd;
        private static SQLiteCommand _getSpawnsNearPointCmd;

        /// <summary>
        /// Indicates if the creature spawns database is available.
        /// </summary>
        public static bool IsAvailable
        {
            get
            {
                EnsureInitialized();
                return _isAvailable;
            }
        }

        private static void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                var dbPath = Path.Combine(Logging.ApplicationPath, "CreatureSpawns.db");
                if (!File.Exists(dbPath))
                {
                    Logging.WriteDebug("[CreatureSpawns] Database not found: {0}", dbPath);
                    return;
                }

                var builder = new SQLiteConnectionStringBuilder
                {
                    DataSource = dbPath,
                    ReadOnly = true
                };

                _connection = new SQLiteConnection(builder.ConnectionString);
                _connection.Open();

                // Initialize SQL commands
                _getSpawnsByEntryCmd = new SQLiteCommand(
                    "SELECT x, y, z FROM spawns WHERE entry = @entry LIMIT 50",
                    _connection);
                _getSpawnsByEntryCmd.Parameters.Add("@entry", System.Data.DbType.Int32);

                _getSpawnsByEntryAndMapCmd = new SQLiteCommand(
                    "SELECT x, y, z FROM spawns WHERE entry = @entry AND map_id = @map_id LIMIT 50",
                    _connection);
                _getSpawnsByEntryAndMapCmd.Parameters.Add("@entry", System.Data.DbType.Int32);
                _getSpawnsByEntryAndMapCmd.Parameters.Add("@map_id", System.Data.DbType.Int32);

                _getSpawnsNearPointCmd = new SQLiteCommand(
                    @"SELECT x, y, z FROM spawns 
                      WHERE entry = @entry AND map_id = @map_id 
                      AND x BETWEEN @minX AND @maxX 
                      AND y BETWEEN @minY AND @maxY
                      LIMIT 25",
                    _connection);
                _getSpawnsNearPointCmd.Parameters.Add("@entry", System.Data.DbType.Int32);
                _getSpawnsNearPointCmd.Parameters.Add("@map_id", System.Data.DbType.Int32);
                _getSpawnsNearPointCmd.Parameters.Add("@minX", System.Data.DbType.Double);
                _getSpawnsNearPointCmd.Parameters.Add("@maxX", System.Data.DbType.Double);
                _getSpawnsNearPointCmd.Parameters.Add("@minY", System.Data.DbType.Double);
                _getSpawnsNearPointCmd.Parameters.Add("@maxY", System.Data.DbType.Double);

                _isAvailable = true;
                Logging.Write("[CreatureSpawns] Database loaded successfully");
            }
            catch (Exception ex)
            {
                Logging.WriteDebug("[CreatureSpawns] Failed to load: {0}", ex.Message);
                _isAvailable = false;
            }
        }

        /// <summary>
        /// Gets all spawn points for a creature entry ID.
        /// </summary>
        /// <param name="entry">The creature entry ID.</param>
        /// <returns>List of spawn points.</returns>
        public static List<WoWPoint> GetSpawnsByEntry(uint entry)
        {
            EnsureInitialized();
            var result = new List<WoWPoint>();

            if (!_isAvailable || _getSpawnsByEntryCmd == null)
                return result;

            try
            {
                _getSpawnsByEntryCmd.Parameters["@entry"].Value = (int)entry;
                using var reader = _getSpawnsByEntryCmd.ExecuteReader();
                while (reader.Read())
                {
                    var point = new WoWPoint(
                        reader.GetFloat(0),
                        reader.GetFloat(1),
                        reader.GetFloat(2));
                    result.Add(point);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDebug("[CreatureSpawns] Error getting spawns for entry {0}: {1}", entry, ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Gets spawn points for a creature entry ID on a specific map.
        /// </summary>
        /// <param name="entry">The creature entry ID.</param>
        /// <param name="mapId">The map ID.</param>
        /// <returns>List of spawn points.</returns>
        public static List<WoWPoint> GetSpawnsByEntryAndMap(uint entry, uint mapId)
        {
            EnsureInitialized();
            var result = new List<WoWPoint>();

            if (!_isAvailable || _getSpawnsByEntryAndMapCmd == null)
                return result;

            try
            {
                _getSpawnsByEntryAndMapCmd.Parameters["@entry"].Value = (int)entry;
                _getSpawnsByEntryAndMapCmd.Parameters["@map_id"].Value = (int)mapId;
                using var reader = _getSpawnsByEntryAndMapCmd.ExecuteReader();
                while (reader.Read())
                {
                    var point = new WoWPoint(
                        reader.GetFloat(0),
                        reader.GetFloat(1),
                        reader.GetFloat(2));
                    result.Add(point);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDebug("[CreatureSpawns] Error getting spawns for entry {0} on map {1}: {2}", 
                    entry, mapId, ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Gets spawn points near a specific location (within search radius).
        /// </summary>
        /// <param name="entry">The creature entry ID.</param>
        /// <param name="mapId">The map ID.</param>
        /// <param name="center">The center point to search around.</param>
        /// <param name="searchRadius">Search radius in yards (default 500).</param>
        /// <returns>List of spawn points within range.</returns>
        public static List<WoWPoint> GetSpawnsNearPoint(uint entry, uint mapId, WoWPoint center, float searchRadius = 500f)
        {
            EnsureInitialized();
            var result = new List<WoWPoint>();

            if (!_isAvailable || _getSpawnsNearPointCmd == null)
                return result;

            try
            {
                _getSpawnsNearPointCmd.Parameters["@entry"].Value = (int)entry;
                _getSpawnsNearPointCmd.Parameters["@map_id"].Value = (int)mapId;
                _getSpawnsNearPointCmd.Parameters["@minX"].Value = center.X - searchRadius;
                _getSpawnsNearPointCmd.Parameters["@maxX"].Value = center.X + searchRadius;
                _getSpawnsNearPointCmd.Parameters["@minY"].Value = center.Y - searchRadius;
                _getSpawnsNearPointCmd.Parameters["@maxY"].Value = center.Y + searchRadius;

                using var reader = _getSpawnsNearPointCmd.ExecuteReader();
                while (reader.Read())
                {
                    var point = new WoWPoint(
                        reader.GetFloat(0),
                        reader.GetFloat(1),
                        reader.GetFloat(2));
                    result.Add(point);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDebug("[CreatureSpawns] Error getting spawns near point: {0}", ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Generates optimized hotspots from spawn points by clustering nearby spawns.
        /// </summary>
        /// <param name="entry">The creature entry ID.</param>
        /// <param name="mapId">The map ID.</param>
        /// <param name="clusterDistance">Distance to cluster spawns together (default 30 yards).</param>
        /// <returns>List of hotspot points (clustered spawn centers).</returns>
        public static List<WoWPoint> GenerateHotspots(uint entry, uint mapId, float clusterDistance = 30f)
        {
            var spawns = GetSpawnsByEntryAndMap(entry, mapId);
            if (spawns.Count == 0)
            {
                Logging.WriteDebug("[CreatureSpawns] No spawns found for entry {0} on map {1}", entry, mapId);
                return new List<WoWPoint>();
            }

            // Cluster spawns into hotspots
            var hotspots = ClusterPoints(spawns, clusterDistance);
            
            Logging.WriteDebug("[CreatureSpawns] Generated {0} hotspots from {1} spawns for entry {2}", 
                hotspots.Count, spawns.Count, entry);
            
            return hotspots;
        }

        /// <summary>
        /// Clusters nearby points into center points.
        /// Uses simple greedy clustering algorithm.
        /// </summary>
        private static List<WoWPoint> ClusterPoints(List<WoWPoint> points, float clusterDistance)
        {
            var clusters = new List<WoWPoint>();
            var used = new bool[points.Count];
            var clusterDistSq = clusterDistance * clusterDistance;

            for (int i = 0; i < points.Count; i++)
            {
                if (used[i]) continue;

                // Start new cluster
                var clusterPoints = new List<WoWPoint> { points[i] };
                used[i] = true;

                // Find all points within cluster distance
                for (int j = i + 1; j < points.Count; j++)
                {
                    if (used[j]) continue;

                    var distSq = points[i].DistanceSqr(points[j]);
                    if (distSq <= clusterDistSq)
                    {
                        clusterPoints.Add(points[j]);
                        used[j] = true;
                    }
                }

                // Calculate cluster center
                float sumX = 0, sumY = 0, sumZ = 0;
                foreach (var p in clusterPoints)
                {
                    sumX += p.X;
                    sumY += p.Y;
                    sumZ += p.Z;
                }
                var center = new WoWPoint(
                    sumX / clusterPoints.Count,
                    sumY / clusterPoints.Count,
                    sumZ / clusterPoints.Count);

                clusters.Add(center);
            }

            return clusters;
        }
    }
}
