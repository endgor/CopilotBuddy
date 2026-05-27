using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Styx.Helpers;
using Styx.Logic.Combat;

namespace Styx.CommonBot.Routines
{
	public class CapabilityManager
	{
		// -----------------------------------------------------------------
		// Singleton
		// -----------------------------------------------------------------

		private static CapabilityManager _instance;

		static CapabilityManager()
		{
			BotEvents.OnBotStopped += args => Instance.Clear(CapabilityFlags.All, "Bot Stopped");
			BotEvents.Profile.OnNewOuterProfileLoaded += args => Instance.Clear(CapabilityFlags.All, "New profile loaded");
		}

		public static CapabilityManager Instance
		{
			get
			{
				if (_instance == null)
					_instance = new CapabilityManager();
				return _instance;
			}
		}

		private CapabilityManager()
		{
		}

		// -----------------------------------------------------------------
		// Internal storage — one dictionary per flag bit (32 slots)
		// -----------------------------------------------------------------

		private readonly Dictionary<CapabilityManagerHandle, ConditionEntry>[] _entries =
			new Dictionary<CapabilityManagerHandle, ConditionEntry>[32];

		// -----------------------------------------------------------------
		// Public API — Update (condition-based)
		// -----------------------------------------------------------------

		public void Update(CapabilityManagerHandle handle, CapabilityFlags capability, Func<bool> condition, string reason = null)
		{
			if (handle == null)    throw new ArgumentNullException("handle");
			if (condition == null) throw new ArgumentNullException("condition");
			ValidateSingleFlag(capability);

			bool isActive = false;
			try   { isActive = condition(); }
			catch (Exception ex) { Logging.WriteException(ex); }

			int slot = GetFlagIndex((uint)capability);
			Dictionary<CapabilityManagerHandle, ConditionEntry> dict = GetOrCreateSlot(slot);

			ConditionEntry existing;
			if (TryGetEntry<ConditionEntry>(dict, handle, out existing))
			{
				existing.Update(condition, reason);
				return;
			}

			if (!isActive) return;

			dict[handle] = new ConditionEntry(condition, reason);
			RoutineManager.SetCapabilityState(capability, CapabilityState.Disallowed, reason);
		}

		// -----------------------------------------------------------------
		// Public API — Update (time-based, ms overload)
		// -----------------------------------------------------------------

		public void Update(CapabilityManagerHandle handle, CapabilityFlags capability, int timeSpanMs, string reason = null)
		{
			Update(handle, capability, TimeSpan.FromMilliseconds(timeSpanMs), reason);
		}

		// -----------------------------------------------------------------
		// Public API — Update (time-based)
		// -----------------------------------------------------------------

		public void Update(CapabilityManagerHandle handle, CapabilityFlags capability, TimeSpan timeSpan, string reason = null)
		{
			if (handle == null) throw new ArgumentNullException("handle");
			ValidateSingleFlag(capability);

			if (timeSpan.TotalMilliseconds <= 0.0) return;

			int slot = GetFlagIndex((uint)capability);
			Dictionary<CapabilityManagerHandle, ConditionEntry> dict = GetOrCreateSlot(slot);

			TimedEntry existing;
			if (TryGetEntry<TimedEntry>(dict, handle, out existing))
			{
				existing.Restart(timeSpan, reason);
				return;
			}

			dict[handle] = new TimedEntry(timeSpan, reason);
			RoutineManager.SetCapabilityState(capability, CapabilityState.Disallowed, reason);
		}

		// -----------------------------------------------------------------
		// Public API — Pulse (evaluate conditions, remove expired entries)
		// -----------------------------------------------------------------

		public void Pulse()
		{
			for (int i = 0; i < _entries.Length; i++)
			{
				Dictionary<CapabilityManagerHandle, ConditionEntry> dict = _entries[i];
				if (dict == null || !dict.Any()) continue;

				CapabilityFlags flag = (CapabilityFlags)(1u << i);
				dict.RemoveAll(entry => !entry.IsActive());

				if (!dict.Any())
					RoutineManager.SetCapabilityState(flag, CapabilityState.DontCare, "All entries in CR CapabilityManager have been removed.");
			}
		}

		// -----------------------------------------------------------------
		// Public API — Clear
		// -----------------------------------------------------------------

		public void Clear(CapabilityFlags capabilities = CapabilityFlags.All, string reason = null)
		{
			Clear(null, capabilities, reason);
		}

		public void Clear(CapabilityManagerHandle handle, CapabilityFlags capabilities = CapabilityFlags.All, string reason = null)
		{
			bool anyCleared = false;
			for (int i = 0; i < _entries.Length; i++)
			{
				CapabilityFlags flag = (CapabilityFlags)(1u << i);
				if ((flag & capabilities) == CapabilityFlags.None) continue;

				Dictionary<CapabilityManagerHandle, ConditionEntry> dict = _entries[i];
				if (dict == null) continue;

				anyCleared = true;
				if (handle != null)
				{
					dict.Remove(handle);
					if (dict.Any()) continue;
				}

				RoutineManager.SetCapabilityState(flag, CapabilityState.DontCare, reason ?? "CR CapabilityManager Cleared");
				_entries[i] = null;
			}

			if (!anyCleared) return;
			LogDiagnostic(string.Format("Cleared {0} entries matching {1}",
				handle == null ? "all" : "handle-matched", capabilities));
		}

		// -----------------------------------------------------------------
		// Public API — Handle factory + Add convenience
		// -----------------------------------------------------------------

		public CapabilityManagerHandle CreateNewHandle()
		{
			return new CapabilityManagerHandle();
		}

		public CapabilityManagerHandle Add(CapabilityFlags capability, Func<bool> condition, string reason = null)
		{
			CapabilityManagerHandle handle = CreateNewHandle();
			Update(handle, capability, condition, reason);
			return handle;
		}

		public CapabilityManagerHandle Add(CapabilityFlags capability, int timeSpanMs, string reason = null)
		{
			return Add(capability, TimeSpan.FromMilliseconds(timeSpanMs), reason);
		}

		public CapabilityManagerHandle Add(CapabilityFlags capability, TimeSpan timeSpan, string reason = null)
		{
			CapabilityManagerHandle handle = CreateNewHandle();
			Update(handle, capability, timeSpan, reason);
			return handle;
		}

		// -----------------------------------------------------------------
		// Private helpers
		// -----------------------------------------------------------------

		private static void LogDiagnostic(string message)
		{
			Logging.WriteDiagnostic("[Capability Manager] " + message);
		}

		/// <summary>Returns the index of the least-significant set bit (= log2 for power-of-2).</summary>
		private static int GetFlagIndex(uint value)
		{
			int n = ((value > 0xFFFFU) ? 1 : 0) << 4;
			value >>= n;
			int t = ((value > 0xFFU) ? 1 : 0) << 3;
			value >>= t;   n |= t;
			t = ((value > 0xFU) ? 1 : 0) << 2;
			value >>= t;   n |= t;
			t = ((value > 0x3U) ? 1 : 0) << 1;
			value >>= t;   n |= t;
			return n | (int)(value >> 1);
		}

		/// <summary>Throws if capability is not exactly one bit.</summary>
		private static void ValidateSingleFlag(CapabilityFlags capability)
		{
			uint v = (uint)capability;
			if (v == 0 || (v & (v - 1)) != 0)
				throw new ArgumentException(string.Format(
					"The {0} capability is not supported. Only single-bit values are allowed.", capability));
		}

		private bool TryGetEntry<T>(Dictionary<CapabilityManagerHandle, ConditionEntry> dict,
			CapabilityManagerHandle handle, out T entry) where T : ConditionEntry
		{
			ConditionEntry raw;
			if (!dict.TryGetValue(handle, out raw))
			{
				entry = default(T);
				return false;
			}
			if (raw.GetType() != typeof(T))
				throw new ArgumentException(string.Format(
					"Handle is associated with an instance of type '{0}' while expected type is {1}",
					raw.GetType(), typeof(T)));
			entry = (T)raw;
			return true;
		}

		private Dictionary<CapabilityManagerHandle, ConditionEntry> GetOrCreateSlot(int slot)
		{
			Dictionary<CapabilityManagerHandle, ConditionEntry> dict = _entries[slot];
			if (dict == null)
				dict = _entries[slot] = new Dictionary<CapabilityManagerHandle, ConditionEntry>();
			return dict;
		}

		// -----------------------------------------------------------------
		// Inner classes — entry types
		// -----------------------------------------------------------------

		private class ConditionEntry
		{
			protected Func<bool> _condition;
			protected string     _reason;

			public ConditionEntry(Func<bool> condition, string reason)
			{
				_condition = condition;
				_reason    = reason;
			}

			// Protected ctor for TimedEntry (sets up condition itself)
			protected ConditionEntry(string reason)
			{
				_reason = reason;
			}

			public virtual bool IsActive()
			{
				try   { return _condition != null && _condition(); }
				catch { return false; }
			}

			public void Update(Func<bool> condition, string reason)
			{
				_condition = condition;
				_reason    = reason;
			}
		}

		private class TimedEntry : ConditionEntry
		{
			private readonly Stopwatch _sw;
			private TimeSpan           _duration;

			public TimedEntry(TimeSpan duration, string reason)
				: base(reason)
			{
				_sw       = Stopwatch.StartNew();
				_duration = duration;
				_condition = IsActive;
			}

			public override bool IsActive()
			{
				return _sw.Elapsed <= _duration;
			}

			public void Restart(TimeSpan duration, string reason)
			{
				_sw.Restart();
				_duration = duration;
				_reason   = reason;
			}
		}
	}
}
