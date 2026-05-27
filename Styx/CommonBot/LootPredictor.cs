using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Styx.CommonBot
{
    /// <summary>
    /// HB 6.2.3: Predicts whether a nearby dead unit is worth looting based on kill history.
    /// Tracks per-entry mob history and avoids wasting time on units that never drop loot.
    /// 25% threshold: if less than 1 in 4 kills yielded loot, skip.
    /// </summary>
    public static class LootPredictor
    {
        private const int WaitTimeoutMs = 1000;
        private const int HistoryExpiryMs = 600000;
        private const int RecordReuseMs = 300000;
        private const double LootThreshold = 0.25;

        public static bool UseLootPredictor { get; set; } = true;

        private static readonly Dictionary<uint, MobHistory> _history = new Dictionary<uint, MobHistory>();
        // GUIDs of units already dead when bot started — skip prediction for these
        private static readonly HashSet<ulong> _preDeadGuids = new HashSet<ulong>();

        // Reuse an expired KillRecord slot, or allocate a new one
        private static KillRecord GetOrCreateRecord(MobHistory history)
        {
            foreach (KillRecord record in history.Records)
            {
                if (ObjectManager.GetObjectByGuid<WoWObject>(record.Guid) == null &&
                    record.Watch.ElapsedMilliseconds > RecordReuseMs)
                {
                    return record;
                }
            }
            KillRecord fresh = new KillRecord();
            history.Records.Add(fresh);
            return fresh;
        }

        private static MobHistory GetOrCreateHistory(uint entry)
        {
            if (!_history.TryGetValue(entry, out MobHistory history))
            {
                history = new MobHistory();
                history.Records = new List<KillRecord>();
                _history[entry] = history;
            }
            history.Watch.Restart();
            return history;
        }

        private static void Cleanup()
        {
            List<uint> expired = _history
                .Where(kvp => kvp.Value.Watch.ElapsedMilliseconds > HistoryExpiryMs)
                .Select(kvp => kvp.Key)
                .ToList();
            foreach (uint key in expired)
                _history.Remove(key);
        }

        /// <summary>Returns true if this unit is worth looting.</summary>
        internal static bool ShouldLoot(WoWUnit unit)
        {
            if (!UseLootPredictor || StyxWoW.Me.IsInParty || _preDeadGuids.Contains(unit.Guid))
                return unit.CanLoot;

            MobHistory history = GetOrCreateHistory(unit.Entry);

            // Existing record for this specific unit instance
            KillRecord existing = history.Records.FirstOrDefault(r => r.Guid == unit.Guid);
            if (existing != null)
            {
                if (unit.CanLoot)
                {
                    if (!existing.HasLootData)
                        existing.HasLootData = true;
                    existing.DidLoot = true;
                    return true;
                }
                // Unit had loot data and we confirmed it had loot → no loot now means done
                if (existing.HasLootData && existing.DidLoot)
                    return false;
                // Give a short grace window before concluding no loot
                if (existing.Watch.ElapsedMilliseconds > WaitTimeoutMs)
                {
                    if (!existing.HasLootData)
                    {
                        existing.DidLoot = false;
                        existing.HasLootData = true;
                    }
                    return false;
                }
                return existing.PredictLoot;
            }

            // No existing record — create one and decide via history
            KillRecord record = GetOrCreateRecord(history);
            record.Guid = unit.Guid;
            record.Watch.Restart();

            if (unit.CanLoot)
            {
                record.DidLoot = true;
                record.PredictLoot = true;
                record.HasLootData = true;
                return true;
            }

            // Predict based on this NPC entry's kill history
            int total = 0;
            int looted = 0;
            foreach (KillRecord r in history.Records)
            {
                bool hadLoot = r.HasLootData ? r.DidLoot : r.PredictLoot;
                total++;
                if (hadLoot) looted++;
            }
            bool predict = total == 1 || (double)looted / total >= LootThreshold;
            record.HasLootData = false;
            record.PredictLoot = predict;
            return predict;
        }

        internal static bool Initialize()
        {
            BotEvents.OnBotStart += OnBotStart;
            BotEvents.OnPulse += OnPulse;
            return true;
        }

        private static void OnPulse(object sender, EventArgs e)
        {
            if (!UseLootPredictor) return;
            Cleanup();
        }

        // Signature: OnBotStartDelegate = void(EventArgs)
        private static void OnBotStart(EventArgs args)
        {
            _preDeadGuids.Clear();
            if (UseLootPredictor)
            {
                _preDeadGuids.UnionWith(
                    ObjectManager.CachedUnits
                        .Where(u => u.IsDead)
                        .Select(u => u.Guid));
            }
        }

        // Per-unit kill record
        private class KillRecord
        {
            public KillRecord() { Watch = new Stopwatch(); }

            public ulong Guid { get; set; }
            public Stopwatch Watch { get; }
            /// <summary>Whether we have confirmed loot outcome (true = seen CanLoot go true or timeout).</summary>
            public bool HasLootData { get; set; }
            /// <summary>Whether the unit actually had loot (CanLoot was true).</summary>
            public bool DidLoot { get; set; }
            /// <summary>Predicted loot outcome used before HasLootData is confirmed.</summary>
            public bool PredictLoot { get; set; }
        }

        // Per-NPC-entry history (multiple kills of same NPC type)
        private class MobHistory
        {
            public MobHistory() { Watch = new Stopwatch(); }

            public List<KillRecord> Records { get; set; }
            public Stopwatch Watch { get; }
        }
    }
}
