using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace WarPilot.Core
{
    // WarPilot-local delegates. Each Routines/ subfolder compiles to its own assembly, so we
    // cannot reuse Singular's SimpleBooleanDelegate / UnitSelectionDelegate — declare our own.
    public delegate bool ReqDelegate(object ctx);
    public delegate WoWUnit UnitDelegate(object ctx);

    /// <summary>
    /// Self-contained spell-cast composite builders for WarPilot. Mirrors the *shape* of
    /// Singular's Spell helper, but calls ONLY DLL-native API (SpellManager / Lua) so it works
    /// inside its own runtime-compiled assembly with no dependency on Singular internals.
    /// </summary>
    public static class WarSpell
    {
        /// <summary>True if the spell is in the spellbook (at any rank).</summary>
        public static bool Known(string name)
        {
            return SpellManager.HasSpell(name);
        }

        /// <summary>Native castability check against a unit (no range/movement gating).</summary>
        public static bool CanCast(string name, WoWUnit unit)
        {
            return unit != null && SpellManager.CanCast(name, unit, false, false);
        }

        /// <summary>Cast on the current target, optionally gated by a requirement. Success only when it fires.</summary>
        public static Composite Cast(string name, ReqDelegate req = null)
        {
            return Cast(name, ctx => StyxWoW.Me.CurrentTarget, req);
        }

        /// <summary>Cast on a chosen unit, optionally gated by a requirement.</summary>
        public static Composite Cast(string name, UnitDelegate onUnit, ReqDelegate req = null)
        {
            return new Decorator(
                ret =>
                {
                    if (req != null && !req(ret)) return false;
                    return CanCast(name, onUnit(ret));
                },
                new Action(ret =>
                {
                    var unit = onUnit(ret);
                    if (unit == null || !SpellManager.Cast(name, unit))
                        return RunStatus.Failure;
                    Logging.WriteDebug("[WarPilot] cast " + name);
                    return RunStatus.Success;
                }));
        }

        /// <summary>Apply a debuff to the current target only if it is not already present (by name).</summary>
        public static Composite Buff(string name, ReqDelegate req = null)
        {
            return new Decorator(
                ret =>
                {
                    var t = StyxWoW.Me.CurrentTarget;
                    if (t == null || t.HasAura(name)) return false;
                    if (req != null && !req(ret)) return false;
                    return CanCast(name, t);
                },
                new Action(ret =>
                {
                    if (!SpellManager.Cast(name, StyxWoW.Me.CurrentTarget))
                        return RunStatus.Failure;
                    Logging.WriteDebug("[WarPilot] debuff " + name);
                    return RunStatus.Success;
                }));
        }

        /// <summary>Apply a self-buff only if it is not already active (by aura name). Not for stances — use WarCommon.EnsureStance.</summary>
        public static Composite BuffSelf(string name, ReqDelegate req = null)
        {
            return new Decorator(
                ret =>
                {
                    var me = StyxWoW.Me;
                    if (me.HasAura(name)) return false;
                    if (req != null && !req(ret)) return false;
                    return CanCast(name, me);
                },
                new Action(ret =>
                {
                    if (!SpellManager.Cast(name, StyxWoW.Me))
                        return RunStatus.Failure;
                    Logging.WriteDebug("[WarPilot] buff " + name);
                    return RunStatus.Success;
                }));
        }
    }
}
