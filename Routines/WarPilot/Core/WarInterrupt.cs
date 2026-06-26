using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using S = WarPilot.Config.WarPilotSettings;

namespace WarPilot.Core
{
    public enum InterruptScope { TargetOnly, TargetAndFocus, AllEnemies }

    /// <summary>
    /// Interrupt manager — a clean-room reimplementation of the design TuanHA's routines used
    /// (the original DLL is DNGuard-protected; only its architecture survived decompilation, see
    /// memory tuanha-warrior-legion-dnguard). It improves on the old single-target WarCommon.CreateInterrupt:
    ///
    ///   * Scope — watch the current target only, target + focus, or ALL nearby enemies in melee.
    ///   * Human-like delay — each enemy cast gets a fresh random reaction delay (min..max ms) so the
    ///     kick doesn't fire on the same frame the cast starts (also dodges fake-cast bait).
    ///   * Min-cast-remaining gate — never kick a cast that is about to finish anyway (wasted cooldown).
    ///   * Except list — a comma-separated spell-name blacklist that is never interrupted.
    ///   * Stance/spell aware — Protection kicks with Shield Bash (stays in Defensive Stance); Arms/Fury
    ///     kick with Pummel, stance-dancing to Berserker (the caller's EnsureStance restores the spec stance).
    ///   * Cooldown aware — checks the interrupt's real cooldown (stance-independent) BEFORE dancing, so a
    ///     Pummel on cooldown never yanks Arms out of Battle Stance for nothing.
    ///
    /// Wholly DLL-native; self-gates on the General "Enable interrupts" setting.
    /// </summary>
    public static class WarInterrupt
    {
        private const int PummelId = 6552;   // Berserker-stance kick (3.3.5a)
        private const float Reach = 8f;       // melee-ish; warrior kicks are melee range

        private static readonly Random Rng = new Random();

        // Per-enemy-cast bookkeeping: when we first noticed THIS cast + the random delay rolled for it.
        private struct Seen { public int CastId; public DateTime First; public int DelayMs; }
        private static readonly Dictionary<ulong, Seen> Tracked = new Dictionary<ulong, Seen>();

        // Chosen action for the current tick (set by the gate predicate, consumed by the cast action).
        private static WoWUnit _victim;
        private static string _spell;
        private static bool _needBerserker;

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Composite Create()
        {
            return new Decorator(
                ret => S.Instance.EnableInterrupts && SelectVictim(),
                new PrioritySelector(
                    // Pummel needs Berserker Stance — dance first, cast next tick once in stance.
                    new Decorator(
                        ret => _needBerserker && Me.Shapeshift != ShapeshiftForm.BerserkerStance,
                        WarCommon.ForceStance(ShapeshiftForm.BerserkerStance, "Berserker Stance")),
                    new Action(ret =>
                    {
                        if (_victim == null) return RunStatus.Failure;
                        WoWMovement.Face(_victim.Guid);
                        if (!SpellManager.Cast(_spell, _victim))
                            return RunStatus.Failure;
                        Logging.Write(System.Drawing.Color.Gold,
                            "[WarPilot] interrupt {0} -> {1} ({2})", _spell, _victim.Name, CastName(_victim));
                        return RunStatus.Success;
                    })));
        }

        /// <summary>Picks the kick spell for the active spec; null when none is known.</summary>
        private static string ChooseSpell(out bool needBerserker)
        {
            // Protection has a shield + lives in Defensive Stance: Shield Bash, no dance.
            if (SpecDetector.Current == WarSpec.Protection && SpellManager.HasSpell("Shield Bash"))
            {
                needBerserker = false;
                return "Shield Bash";
            }
            if (SpellManager.HasSpell("Pummel"))
            {
                needBerserker = true;
                return "Pummel";
            }
            // Last resort: a known Shield Bash even off-spec (e.g. an Arms warrior carrying a shield).
            if (SpellManager.HasSpell("Shield Bash"))
            {
                needBerserker = false;
                return "Shield Bash";
            }
            needBerserker = false;
            return null;
        }

        /// <summary>
        /// Finds an enemy whose interruptible cast is past its reaction delay and still has enough time
        /// left to be worth kicking. Side-effects <see cref="_victim"/>/<see cref="_spell"/>/<see cref="_needBerserker"/>.
        /// </summary>
        private static bool SelectVictim()
        {
            _victim = null;
            _spell = ChooseSpell(out _needBerserker);
            if (_spell == null) return false;

            // Stance-independent cooldown check — don't dance for a kick that isn't ready.
            if (CooldownLeft(_spell) > TimeSpan.FromMilliseconds(250)) return false;

            var s = S.Instance;
            foreach (var u in Candidates(s.InterruptScope))
            {
                if (u == null || !u.IsValid || u.Dead) continue;
                if (u.Distance > Reach) continue;
                if (!u.CanInterruptCurrentSpellCast) continue;
                if (!(u.IsCasting || u.IsChanneling)) continue;

                string castName = CastName(u);
                if (IsExcepted(castName, s.InterruptExcept)) { Forget(u.Guid); continue; }

                int left = CastLeftMs(u);
                if (left < s.InterruptMinCastLeftMs) continue;   // about to finish — let it

                if (!DelayElapsed(u, s.InterruptDelayMin, s.InterruptDelayMax)) continue;

                _victim = u;
                return true;
            }
            return false;
        }

        private static IEnumerable<WoWUnit> Candidates(InterruptScope scope)
        {
            var me = StyxWoW.Me;
            switch (scope)
            {
                case InterruptScope.TargetOnly:
                    if (me.CurrentTarget != null) yield return me.CurrentTarget;
                    break;
                case InterruptScope.TargetAndFocus:
                    if (me.CurrentTarget != null) yield return me.CurrentTarget;
                    if (me.FocusedUnit != null) yield return me.FocusedUnit;
                    break;
                default: // AllEnemies — any attackable unit in melee
                    foreach (var u in ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                                 .Where(x => x.Attackable && x.CanSelect && !x.Dead && x.Distance <= Reach)
                                 .OrderBy(x => x.Distance))
                        yield return u;
                    break;
            }
        }

        // --- cast inspection helpers ---

        private static WoWSpell CastSpell(WoWUnit u)
        {
            return u.IsChanneling ? u.ChanneledCastingSpell : u.CastingSpell;
        }

        private static string CastName(WoWUnit u)
        {
            var sp = CastSpell(u);
            return sp != null ? sp.Name : string.Empty;
        }

        private static int CastId(WoWUnit u)
        {
            return u.IsChanneling ? u.ChanneledCastingSpellId : u.CastingSpellId;
        }

        private static int CastLeftMs(WoWUnit u)
        {
            var ts = u.IsChanneling ? u.CurrentChannelTimeLeft : u.CurrentCastTimeLeft;
            return (int)ts.TotalMilliseconds;
        }

        private static bool IsExcepted(string castName, string exceptCsv)
        {
            if (string.IsNullOrEmpty(castName) || string.IsNullOrWhiteSpace(exceptCsv)) return false;
            foreach (var raw in exceptCsv.Split(','))
            {
                var token = raw.Trim();
                if (token.Length > 0 && castName.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>Rolls a fresh reaction delay the first time we see a cast and reports once it has elapsed.</summary>
        private static bool DelayElapsed(WoWUnit u, int minMs, int maxMs)
        {
            if (maxMs < minMs) maxMs = minMs;
            int castId = CastId(u);
            Seen seen;
            if (!Tracked.TryGetValue(u.Guid, out seen) || seen.CastId != castId)
            {
                seen = new Seen { CastId = castId, First = DateTime.Now, DelayMs = Rng.Next(minMs, maxMs + 1) };
                Tracked[u.Guid] = seen;
            }
            return (DateTime.Now - seen.First).TotalMilliseconds >= seen.DelayMs;
        }

        private static void Forget(ulong guid) { Tracked.Remove(guid); }

        private static TimeSpan CooldownLeft(string name)
        {
            WoWSpell s;
            if (SpellManager.Spells.TryGetValue(name, out s) && s != null)
                return s.CooldownTimeLeft;
            return TimeSpan.Zero;
        }
    }
}
