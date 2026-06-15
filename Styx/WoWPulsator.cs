using System;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Plugins;
using Styx.WoWInternals;

namespace Styx
{
	/// <summary>
	/// HB 6.2.3 Pulsator.Pulse — ported verbatim from
	/// C:\Users\Texy6\Desktop\newhcb\hb decompile\.hb 6.2.3\Honorbuddy\Styx\Pulsator.cs
	///
	/// Pulse order matches HB 6.2.3 exactly:
	///   Objects   → ObjectManager.Update + Blacklist.Flush
	///   Lua       → Lua.ProcessEvents
	///   InfoPanel → InfoPanel.Update
	///   Looting   → LootTargeting.Pulse
	///   Targeting → Targeting.Pulse + HealTargeting.Pulse
	///   BotEvents → BotEvents.PulseEvents
	///   Plugins   → PluginManager.Pulse
	///   Routine   → CapabilityManager.Pulse + RoutineManager.Current.Pulse
	///
	/// Removed (not in HB 6.2.3): WoWChat, WoWMovement.Pulse, Mount.Pulse,
	/// AvoidanceManager.Pulse, NavAvoidanceUpdater.Invoke.
	/// </summary>
	public static class WoWPulsator
	{
		public static void Pulse(PulseFlags flags)
		{
			try
			{
				if ((flags & PulseFlags.Objects) != (PulseFlags)0U)
				{
					ObjectManager.Update();
					Blacklist.Flush();
				}

				if ((flags & PulseFlags.Lua) != (PulseFlags)0U)
				{
					Lua.ProcessEvents();
				}

				if ((flags & PulseFlags.InfoPanel) != (PulseFlags)0U)
				{
					InfoPanel.Update();
				}

				if ((flags & PulseFlags.Looting) != (PulseFlags)0U)
				{
					LootTargeting.Instance.Pulse();
				}

				if ((flags & PulseFlags.Targeting) != (PulseFlags)0U)
				{
					Targeting.Instance.Pulse();
					HealTargeting.Instance.Pulse();
				}


				if ((flags & PulseFlags.BotEvents) != (PulseFlags)0U)
				{
					BotEvents.PulseEvents();
				}

				if ((flags & PulseFlags.Plugins) != (PulseFlags)0U)
				{
					PluginManager.Pulse();
				}

				if (RoutineManager.Current != null)
				{
					CapabilityManager.Instance.Pulse();
					RoutineManager.Current.Pulse();
				}
			}
			catch (Exception ex)
			{
				Logging.WriteException(ex);
			}
		}
	}
}
