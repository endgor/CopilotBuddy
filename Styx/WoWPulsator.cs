using System;
using System.Diagnostics;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Plugins;
using Styx.WoWInternals;

namespace Styx
{
	public static class WoWPulsator
	{
		public static void Pulse(PulseFlags flags)
		{
			try
			{
				var stageTimer = Stopwatch.StartNew();

				if ((flags & PulseFlags.Objects) != (PulseFlags)0U)
				{
					stageTimer.Restart();
					ObjectManager.Update();
					long objectUpdateMs = stageTimer.ElapsedMilliseconds;
					if (objectUpdateMs >= 25)
						Logging.WriteDiagnostic("[TICK] ObjectManager.Update: {0}ms", objectUpdateMs);
				}

				if ((flags & PulseFlags.Lua) != (PulseFlags)0U)
				{
					stageTimer.Restart();
					Lua.ProcessEvents();
					long luaEventsMs = stageTimer.ElapsedMilliseconds;
					if (luaEventsMs >= 25)
						Logging.WriteDiagnostic("[TICK] Lua.ProcessEvents: {0}ms", luaEventsMs);
				}

				if ((flags & PulseFlags.WoWChat) != (PulseFlags)0U)
				{
					stageTimer.Restart();
					WoWChat.Update();
					long wowChatMs = stageTimer.ElapsedMilliseconds;
					if (wowChatMs >= 25)
						Logging.WriteDiagnostic("[TICK] WoWChat.Update: {0}ms", wowChatMs);
				}

				if ((flags & PulseFlags.InfoPanel) != (PulseFlags)0U)
				{
					stageTimer.Restart();
					InfoPanel.Update();
					long infoPanelMs = stageTimer.ElapsedMilliseconds;
					if (infoPanelMs >= 25)
						Logging.WriteDiagnostic("[TICK] InfoPanel.Update: {0}ms", infoPanelMs);
				}

				if ((flags & PulseFlags.Looting) != (PulseFlags)0U)
				{
					stageTimer.Restart();
					LootTargeting.Instance.Pulse();
					long lootingMs = stageTimer.ElapsedMilliseconds;
					if (lootingMs >= 25)
						Logging.WriteDiagnostic("[TICK] LootTargeting.Pulse: {0}ms", lootingMs);
				}

				if ((flags & PulseFlags.Targeting) != (PulseFlags)0U)
				{
					Targeting.Instance.Pulse();
				}

				// BUG-07 fix: Pulse movement to flush timed movement entries
				stageTimer.Restart();
				WoWMovement.Pulse();
				long movementMs = stageTimer.ElapsedMilliseconds;
				if (movementMs >= 25)
					Logging.WriteDiagnostic("[TICK] WoWMovement.Pulse: {0}ms", movementMs);

				// Required for StuckHandler.OnMountUp cancellation to work.
				// Without this, the OnMountUp event never fires and the 10-second
				// mount-block after a stuck dismount has no effect.
				stageTimer.Restart();
				Mount.Pulse();
				long mountPulseMs = stageTimer.ElapsedMilliseconds;
				if (mountPulseMs >= 25)
					Logging.WriteDiagnostic("[TICK] Mount.Pulse: {0}ms", mountPulseMs);

				// BUG-07 fix: Pulse avoidance zones (was missing per audit)
				stageTimer.Restart();
				Styx.Logic.Pathing.AvoidanceManager.Pulse();
				long mobAvoidanceMs = stageTimer.ElapsedMilliseconds;
				if (mobAvoidanceMs >= 25)
					Logging.WriteDiagnostic("[TICK] Pathing.AvoidanceManager.Pulse: {0}ms", mobAvoidanceMs);

				// HB 6.2.3 AvoidanceNavigationProvider pattern:
				// Update geometric obstacle avoidance zones so Navigator.MoveTo()
				// can redirect the bot around registered world obstacles (forge, mailbox, etc.).
				// Set by WorldObstacleManager.Initialize() — no-op when no bots have registered.
				stageTimer.Restart();
				Styx.Logic.Pathing.Navigator.NavAvoidanceUpdater?.Invoke();
				long navAvoidanceMs = stageTimer.ElapsedMilliseconds;
				if (navAvoidanceMs >= 25)
					Logging.WriteDiagnostic("[TICK] Navigator.NavAvoidanceUpdater: {0}ms", navAvoidanceMs);

				if ((flags & PulseFlags.BotEvents) != (PulseFlags)0U)
				{
					stageTimer.Restart();
					BotEvents.PulseEvents();
					long botEventsMs = stageTimer.ElapsedMilliseconds;
					if (botEventsMs >= 25)
						Logging.WriteDiagnostic("[TICK] BotEvents.PulseEvents: {0}ms", botEventsMs);
				}

				if ((flags & PulseFlags.Plugins) != (PulseFlags)0U)
				{
					stageTimer.Restart();
					PluginManager.Pulse();
					long pluginMs = stageTimer.ElapsedMilliseconds;
					if (pluginMs >= 25)
						Logging.WriteDiagnostic("[TICK] PluginManager.Pulse: {0}ms", pluginMs);
				}

				if (RoutineManager.Current != null)
				{
					stageTimer.Restart();
					RoutineManager.Current.Pulse();
					long routineMs = stageTimer.ElapsedMilliseconds;
					if (routineMs >= 25)
						Logging.WriteDiagnostic("[TICK] Routine.Pulse: {0}ms", routineMs);
				}
			}
			catch (Exception ex)
			{
				Logging.WriteException(ex);
			}
		}
	}
}
