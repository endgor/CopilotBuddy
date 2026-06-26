using System;
using System.Drawing;
using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using S = WarPilot.Config.WarPilotSettings;

namespace WarPilot.Core
{
    /// <summary>
    /// Burst manager — the TuanHA "(X on cooldown) OR (X on burst &amp;&amp; BurstManager.Burst)" idea
    /// (see memory tuanha-warrior-legion-dnguard), reimplemented clean. Two ways burst turns on:
    ///
    ///   * Manual — a global hotkey (default Ctrl+Shift+B) toggles a burst window on/off.
    ///   * Automatic — when "Auto-burst" is enabled and the target is worth it (elite / boss / player),
    ///     so cooldowns aren't wasted on trash while leveling.
    ///
    /// <see cref="Create"/> fires the off-GCD, cross-spec burst pieces — offensive racials (Blood Fury /
    /// Berserking, gated by HasSpell so race is implicit) and on-use trinkets — and returns Failure so the
    /// rotation still acts on the same tick. Spec rotations read <see cref="Active"/> to also unleash their
    /// own damage cooldown (e.g. Arms Recklessness) during a manual burst.
    /// </summary>
    public static class WarBurst
    {
        private const string HotkeyName = "WarPilot Burst";
        private static bool _registered;
        private static bool _manualBurst;

        // Don't hammer trinket.Use() every tick — one attempt per second is plenty.
        private static DateTime _nextTrinket = DateTime.MinValue;

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static bool ManualBurst { get { return _manualBurst; } }

        public static void EnsureHotkey()
        {
            if (_registered || !S.Instance.EnableBurstHotkey) return;
            try
            {
                HotkeysManager.Register(HotkeyName, Keys.B, ModifierKeys.Control | ModifierKeys.Shift, hk => Toggle());
                _registered = true;
                Logging.Write(Color.FromArgb(200, 64, 47),
                    "[WarPilot] burst hotkey registered: Ctrl+Shift+B (toggles a burst window).");
            }
            catch (Exception ex)
            {
                Logging.Write(Color.OrangeRed, "[WarPilot] could not register burst hotkey: " + ex.Message);
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
            _manualBurst = !_manualBurst;
            Logging.Write(_manualBurst ? Color.Gold : Color.Gray,
                "[WarPilot] burst " + (_manualBurst ? "ON" : "off") + " (Ctrl+Shift+B).");
        }

        /// <summary>True while cooldowns should be unleashed (manual window, or auto on a worthy target).</summary>
        public static bool Active
        {
            get
            {
                if (_manualBurst) return true;
                if (!S.Instance.AutoBurst) return false;
                var t = Me.CurrentTarget;
                return t != null && (t.Elite || t.IsBoss || t.IsPlayer);
            }
        }

        /// <summary>
        /// Off-GCD racials + trinkets. Every action returns Failure even after firing, so the manager is
        /// non-blocking — a burst racial/trinket goes off WITHOUT costing the tick's GCD ability.
        /// </summary>
        public static Composite Create()
        {
            return new Decorator(ret => Me.GotTarget && Active, new PrioritySelector(
                // Offensive racials — HasSpell makes the race implicit (Orc=Blood Fury, Troll=Berserking).
                FireRacial("Blood Fury"),
                FireRacial("Berserking"),
                // On-use trinkets (throttled so we don't spam Use()).
                new Action(ret =>
                {
                    if (S.Instance.UseTrinkets && DateTime.Now >= _nextTrinket)
                    {
                        _nextTrinket = DateTime.Now + TimeSpan.FromSeconds(1);
                        TryUse(Me.Inventory.Equipped.Trinket1);
                        TryUse(Me.Inventory.Equipped.Trinket2);
                    }
                    return RunStatus.Failure;
                })));
        }

        private static Composite FireRacial(string name)
        {
            return new Action(ret =>
            {
                if (S.Instance.UseRacials && !Me.HasAura(name) && SpellManager.CanCast(name, Me, false, false))
                {
                    if (SpellManager.Cast(name, Me))
                        Logging.Write(Color.Gold, "[WarPilot] burst racial: " + name);
                }
                return RunStatus.Failure; // never blocks the rotation
            });
        }

        private static void TryUse(WoWItem trinket)
        {
            if (trinket == null || !trinket.Usable || trinket.CooldownTimeLeft > TimeSpan.Zero) return;
            trinket.Use();
            Logging.Write(Color.Gold, "[WarPilot] burst trinket: " + trinket.Name);
        }
    }
}
