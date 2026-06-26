using System.Collections.Generic;
using System.Drawing;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Combat;
using TreeSharp;
using Action = TreeSharp.Action;
using WarPilot.Config;

namespace WarPilot.Core
{
    /// <summary>
    /// Cross-spec warrior utilities: rage, stance, shout/auto-attack upkeep and the on-next-swing
    /// gate. All DLL-native; safe to call from any spec rotation. This is the single source for the
    /// player accessor (<see cref="Me"/>) and warrior upkeep policy used by every spec.
    /// </summary>
    public static class WarCommon
    {
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        // WotLK API has no RagePercent on the player — compute it (Singular does the same).
        public static double RagePercent
        {
            get
            {
                var me = StyxWoW.Me;
                return me.MaxRage <= 0 ? 0 : (me.CurrentRage / (double)me.MaxRage) * 100.0;
            }
        }

        public static int Rage { get { return StyxWoW.Me.CurrentRage; } }

        /// <summary>SpellManager.GlobalCooldown reads the client cooldown list — "is the GCD active".</summary>
        public static bool IsGlobalCooldown { get { return SpellManager.GlobalCooldown; } }

        // Heroic Strike / Cleave are "on next melee swing": they never trigger the GCD and never go on
        // cooldown, so CanCast stays true every pulse until the next white swing consumes the queued
        // attack. Without this gate the routine re-queues (and log-spams) ~10x/sec. One Lua round-trip
        // (IsCurrentSpell) covers both on-swing abilities.
        public static bool CanQueueOnSwing
        {
            get
            {
                return Lua.GetReturnVal<int>(
                    "return (IsCurrentSpell(\"Heroic Strike\") or IsCurrentSpell(\"Cleave\")) and 1 or 0", 0) == 0;
            }
        }

        /// <summary>Switch to the given stance if not already in it and it is learned (ignores KeepStance).</summary>
        public static Composite ForceStance(ShapeshiftForm form, string stanceName)
        {
            return new Decorator(
                ret => StyxWoW.Me.Shapeshift != form && SpellManager.HasSpell(stanceName),
                new Action(ret =>
                {
                    SpellManager.Cast(stanceName);
                    return RunStatus.Success;
                }));
        }

        /// <summary>
        /// Keep the spec's stance — only when KeepStance is enabled. Delegates to <see cref="ForceStance"/>
        /// so every spec's stance handling shares one implementation.
        /// </summary>
        public static Composite EnsureStance(ShapeshiftForm form, string stanceName)
        {
            return new Decorator(ret => WarPilotSettings.Instance.KeepStance, ForceStance(form, stanceName));
        }

        /// <summary>True when the unit is casting/channeling something we are allowed to interrupt.</summary>
        public static bool ShouldInterrupt(WoWUnit u)
        {
            return u != null && (u.IsCasting || u.IsChanneling) && u.CanInterruptCurrentSpellCast;
        }

        /// <summary>
        /// Interrupt the current target's interruptible cast with <paramref name="interruptSpell"/>, dancing
        /// into <paramref name="castStance"/> first if the interrupt requires it (e.g. Pummel → Berserker).
        /// The caller's own EnsureStance restores the spec stance once the cast is gone. Cross-spec: Prot can
        /// reuse this for Shield Bash / Pummel without re-implementing the dance.
        /// </summary>
        public static Composite CreateInterrupt(string interruptSpell, ShapeshiftForm castStance, string castStanceName)
        {
            return new Decorator(
                ret => StyxWoW.Me.GotTarget && SpellManager.HasSpell(interruptSpell) && ShouldInterrupt(StyxWoW.Me.CurrentTarget),
                new PrioritySelector(
                    WarSpell.Cast(interruptSpell, ret => StyxWoW.Me.Shapeshift == castStance),
                    ForceStance(castStance, castStanceName)));
        }

        /// <summary>Keep the configured shout up — Commanding or Battle, exactly one (single source of the either/or rule).</summary>
        public static Composite MaintainShout()
        {
            return new PrioritySelector(
                WarSpell.BuffSelf("Commanding Shout", ret => WarPilotSettings.Instance.UseCommandingShout),
                WarSpell.BuffSelf("Battle Shout", ret => !WarPilotSettings.Instance.UseCommandingShout));
        }

        /// <summary>Ensure melee auto-attack is turned on against the current target.</summary>
        public static Composite CreateAutoAttack()
        {
            return new Decorator(
                ret => StyxWoW.Me.GotTarget && !StyxWoW.Me.IsAutoAttacking,
                new Action(ret =>
                {
                    StyxWoW.Me.ToggleAttack();
                    return RunStatus.Failure; // never blocks the rotation
                }));
        }

        // One-time notice for unfinished specs (Prot/Fury stubs). Logs each distinct message once.
        private static readonly HashSet<string> _warned = new HashSet<string>();
        public static Composite WarnOnce(string message)
        {
            return new Action(ret =>
            {
                if (_warned.Add(message))
                    Logging.Write(Color.Orange, message);
                return RunStatus.Failure;
            });
        }
    }
}
