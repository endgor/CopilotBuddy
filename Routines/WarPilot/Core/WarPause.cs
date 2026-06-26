using System;
using System.Drawing;
using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.Helpers;
using TreeSharp;
using Action = TreeSharp.Action;
using S = WarPilot.Config.WarPilotSettings;

namespace WarPilot.Core
{
    /// <summary>
    /// Pause manager — distilled from TuanHA's PauseManager (see memory tuanha-warrior-legion-dnguard).
    /// Two ways the rotation is suspended:
    ///
    ///   * Manual — a global hotkey (default Ctrl+Shift+P) toggles the routine on/off without unloading
    ///     it, so you can take manual control mid-fight and hand it back. Registered once at Initialize.
    ///   * Automatic — when the character is mind-controlled/possessed or riding a vehicle, the warrior
    ///     rotation would either fight your own allies or fire warrior spells that don't apply. We hold.
    ///
    /// <see cref="Gate"/> goes FIRST in every behavior tree: while paused it returns Success, which stops
    /// the parent PrioritySelector so nothing past it runs.
    /// </summary>
    public static class WarPause
    {
        private const string HotkeyName = "WarPilot Pause";
        private static bool _registered;
        private static bool _manualPaused;

        public static bool ManualPaused { get { return _manualPaused; } }

        /// <summary>Register the global pause hotkey once. Safe to call repeatedly.</summary>
        public static void EnsureHotkey()
        {
            if (_registered || !S.Instance.EnablePauseHotkey) return;
            try
            {
                HotkeysManager.Register(HotkeyName, Keys.P, ModifierKeys.Control | ModifierKeys.Shift, hk => Toggle());
                _registered = true;
                Logging.Write(Color.FromArgb(200, 64, 47),
                    "[WarPilot] pause hotkey registered: Ctrl+Shift+P (toggles the rotation on/off).");
            }
            catch (Exception ex)
            {
                Logging.Write(Color.OrangeRed, "[WarPilot] could not register pause hotkey: " + ex.Message);
            }
        }

        public static void RemoveHotkey()
        {
            if (!_registered) return;
            try { HotkeysManager.Unregister(HotkeyName); } catch { }
            _registered = false;
        }

        private static void Toggle()
        {
            _manualPaused = !_manualPaused;
            Logging.Write(_manualPaused ? Color.Orange : Color.LightGreen,
                "[WarPilot] " + (_manualPaused ? "PAUSED (Ctrl+Shift+P to resume)." : "resumed."));
        }

        public static bool IsPaused
        {
            get
            {
                if (_manualPaused) return true;
                var me = StyxWoW.Me;
                if (me == null) return false;
                if (S.Instance.AutoPauseWhenControlled && (me.Possessed || me.InVehicle))
                    return true;
                return false;
            }
        }

        /// <summary>Top-of-tree halt: returns Success while paused so the rest of the tree is skipped.</summary>
        public static Composite Gate()
        {
            return new Decorator(ret => IsPaused, new Action(ret => RunStatus.Success));
        }
    }
}
