using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using PallyPilot.Config;
using S = PallyPilot.Config.PallyPilotSettings;

namespace PallyPilot.Core
{
    /// <summary>
    /// Cross-spec paladin utilities: the player accessor, mana, GCD, and the shared upkeep policy for
    /// Blessings / Auras / Seals plus the dispel (Cleanse / Purify) engine. All DLL-native; usable from
    /// any spec rotation. This is the single source of the auto-buff rules so Ret and Holy stay in sync.
    /// </summary>
    public static class PalCommon
    {
        public static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static S Cfg { get { return S.Instance; } }

        public static double ManaPercent { get { return StyxWoW.Me.ManaPercent; } }
        public static bool IsGlobalCooldown { get { return SpellManager.GlobalCooldown; } }

        // ---------------- Blessing ----------------

        /// <summary>Resolve the configured blessing to a concrete spell name. Auto = Kings if known, else Might.</summary>
        public static string ResolveBlessing()
        {
            switch (Cfg.Blessing)
            {
                case BlessingChoice.Might:      return "Blessing of Might";
                case BlessingChoice.Kings:      return "Blessing of Kings";
                case BlessingChoice.Wisdom:     return "Blessing of Wisdom";
                case BlessingChoice.Sanctuary:  return "Blessing of Sanctuary";
                default:
                    return PalSpell.Known("Blessing of Kings") ? "Blessing of Kings" : "Blessing of Might";
            }
        }

        /// <summary>Keep the configured self-blessing up (a paladin can only have one of their own blessings active).</summary>
        public static Composite MaintainBlessing()
        {
            return new Decorator(ret => Cfg.KeepBlessing,
                PalSpell.BuffSelf(ResolveBlessing()));
        }

        // ---------------- Aura ----------------

        public static string ResolveAura()
        {
            switch (Cfg.Aura)
            {
                case AuraChoice.Devotion:       return "Devotion Aura";
                case AuraChoice.Retribution:    return "Retribution Aura";
                case AuraChoice.Concentration:  return "Concentration Aura";
                default:
                    if (SpecDetector.Current == PalSpec.Retribution && PalSpell.Known("Retribution Aura"))
                        return "Retribution Aura";
                    if (SpecDetector.Current == PalSpec.Holy && PalSpell.Known("Concentration Aura"))
                        return "Concentration Aura";
                    return "Devotion Aura";
            }
        }

        /// <summary>Keep the configured aura up (auras are exclusive — casting one replaces the current).</summary>
        public static Composite MaintainAura()
        {
            return new Decorator(ret => Cfg.KeepAura,
                PalSpell.BuffSelf(ResolveAura()));
        }

        // ---------------- Seal ----------------

        /// <summary>Resolve the Retribution seal. Auto = Command if known, else Righteousness; Vengeance/Corruption is faction-aware.</summary>
        public static string ResolveRetSeal()
        {
            switch (Cfg.RetSeal)
            {
                case RetSealChoice.Command:             return "Seal of Command";
                case RetSealChoice.Righteousness:       return "Seal of Righteousness";
                case RetSealChoice.Wisdom:              return "Seal of Wisdom";
                case RetSealChoice.Light:               return "Seal of Light";
                case RetSealChoice.Martyr:              return StyxWoW.Me.IsHorde ? "Seal of Blood" : "Seal of the Martyr";
                case RetSealChoice.VengeanceCorruption: return StyxWoW.Me.IsHorde ? "Seal of Corruption" : "Seal of Vengeance";
                default:
                    return PalSpell.Known("Seal of Command") ? "Seal of Command" : "Seal of Righteousness";
            }
        }

        /// <summary>Keep the given seal up as a self-buff (seals last 30 min; recasting replaces).</summary>
        public static Composite MaintainSeal(string sealName)
        {
            return PalSpell.BuffSelf(sealName);
        }

        /// <summary>Resolve the Holy seal: Wisdom (mana return) if known, else Light, else Righteousness.</summary>
        public static string ResolveHolySeal()
        {
            if (PalSpell.Known("Seal of Wisdom")) return "Seal of Wisdom";
            if (PalSpell.Known("Seal of Light")) return "Seal of Light";
            return "Seal of Righteousness";
        }

        public static string ResolveJudgement()
        {
            switch (Cfg.RetJudgement)
            {
                case JudgementChoice.Light:   return "Judgement of Light";
                case JudgementChoice.Justice: return "Judgement of Justice";
                default:                      return "Judgement of Wisdom";
            }
        }

        // ---------------- Cleanse / Purify ----------------

        /// <summary>The dispel spell we'll use — Cleanse (adds Magic) if known, else Purify.</summary>
        private static string CleanseSpell()
        {
            return PalSpell.Known("Cleanse") ? "Cleanse" : "Purify";
        }

        private static bool DispelAllowed(WoWDispelType type, bool withMagic)
        {
            if (type == WoWDispelType.Poison || type == WoWDispelType.Disease) return true;
            if (withMagic && type == WoWDispelType.Magic) return true;
            return false;
        }

        // One-time diagnostic: logs each distinct harmful aura the bot sees + the dispel type it reads,
        // so we can tell whether a "poison" is actually flagged dispellable in the 3.3.5a spell data.
        private static readonly HashSet<string> _debuffLog = new HashSet<string>();

        /// <summary>True if the unit carries a debuff this paladin can remove with the current cleanse spell.</summary>
        public static bool HasDispellableDebuff(WoWUnit u)
        {
            if (u == null || u.Dead) return false;
            bool withMagic = PalSpell.Known("Cleanse") && Cfg.CleanseMagic;
            try
            {
                // ActiveAuras (not just .Debuffs) so a harmful aura mis-flagged as a buff is still seen.
                foreach (var aura in u.GetAllAuras().ActiveAuras)
                {
                    if (aura == null) continue;
                    var type = aura.Spell != null ? aura.Spell.DispelType : WoWDispelType.None;

                    string key = aura.Name + "|" + type + "|" + aura.IsHarmful;
                    if (_debuffLog.Add(key))
                        Logging.Write("[PallyPilot] aura on " + u.SafeName() + ": '" + aura.Name +
                            "' harmful=" + aura.IsHarmful + " dispelType=" + type +
                            (aura.Spell == null ? " (no spell record)" : ""));

                    if (aura.IsHarmful && DispelAllowed(type, withMagic)) return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Cleanse the first dispellable friendly unit among <paramref name="friends"/>. Gated by the
        /// cleanse mana floor so it never strands the paladin out of mana. Shared by Ret (self only) and
        /// Holy (whole group).
        /// </summary>
        public static Composite CreateCleanse(Func<IEnumerable<WoWUnit>> friends)
        {
            return new Decorator(
                ret => Cfg.AutoCleanse && ManaPercent >= Cfg.CleanseMinMana,
                new Action(ret =>
                {
                    var spell = CleanseSpell();
                    if (!PalSpell.Known(spell)) return RunStatus.Failure;

                    var victim = friends().FirstOrDefault(HasDispellableDebuff);
                    if (victim == null || !PalSpell.CanCast(spell, victim))
                        return RunStatus.Failure;

                    if (!SpellManager.Cast(spell, victim))
                        return RunStatus.Failure;
                    Logging.Write("[PallyPilot] " + spell + " (cleanse) on " + victim.SafeName());
                    return RunStatus.Success;
                }));
        }

        // ---------------- misc ----------------

        /// <summary>
        /// Ensure melee auto-attack is turned on against the current target. Returns Failure so it never
        /// blocks the rest of the rotation — it just guarantees white swings (which also proc seals). This
        /// is most of a low-level paladin's damage before Crusader Strike is learned.
        /// </summary>
        public static Composite CreateAutoAttack()
        {
            return new Decorator(
                ret => StyxWoW.Me.GotTarget && !StyxWoW.Me.IsAutoAttacking,
                new Action(ret =>
                {
                    StyxWoW.Me.ToggleAttack();
                    return RunStatus.Failure;
                }));
        }

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
