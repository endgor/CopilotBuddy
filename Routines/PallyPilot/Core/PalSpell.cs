using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace PallyPilot.Core
{
    // PallyPilot-local delegates. Each Routines/ subfolder compiles to its own assembly, so we cannot
    // reuse Singular's SimpleBooleanDelegate / UnitSelectionDelegate — declare our own.
    public delegate bool ReqDelegate(object ctx);
    public delegate WoWUnit UnitDelegate(object ctx);

    /// <summary>
    /// Self-contained spell-cast composite builders for PallyPilot. Mirrors the shape of WarPilot's
    /// WarSpell / Singular's Spell helper, but calls ONLY DLL-native API (SpellManager) so it works
    /// inside its own runtime-compiled assembly. Adds friendly-target casting (heals / blessings /
    /// beacon) on top of the offensive builders.
    /// </summary>
    public static class PalSpell
    {
        public static bool Known(string name) { return SpellManager.HasSpell(name); }

        public static bool CanCast(string name, WoWUnit unit)
        {
            return unit != null && SpellManager.CanCast(name, unit, false, false);
        }

        public static bool CanCast(int spellId, WoWUnit unit)
        {
            return unit != null && SpellManager.CanCast(spellId, unit, false, false);
        }

        // ---------------- offensive (current target) ----------------

        /// <summary>Cast on the current target, optionally gated by a requirement.</summary>
        public static Composite Cast(string name, ReqDelegate req = null)
        {
            return CastOn(name, ctx => StyxWoW.Me.CurrentTarget, req);
        }

        /// <summary>Cast on a chosen unit (friendly or hostile), optionally gated by a requirement.</summary>
        public static Composite CastOn(string name, UnitDelegate onUnit, ReqDelegate req = null)
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
                    Logging.WriteDebug("[PallyPilot] cast " + name + " on " + unit.SafeName());
                    return RunStatus.Success;
                }));
        }

        /// <summary>Cast a SPECIFIC rank (by spell id) on a unit. Used by the heal downranking path.</summary>
        public static Composite CastIdOn(IdDelegate onId, UnitDelegate onUnit, ReqDelegate req = null)
        {
            return new Decorator(
                ret =>
                {
                    if (req != null && !req(ret)) return false;
                    int id = onId(ret);
                    return id != 0 && CanCast(id, onUnit(ret));
                },
                new Action(ret =>
                {
                    int id = onId(ret);
                    var unit = onUnit(ret);
                    if (id == 0 || unit == null || !SpellManager.Cast(id, unit))
                        return RunStatus.Failure;
                    Logging.WriteDebug("[PallyPilot] cast id " + id + " on " + unit.SafeName());
                    return RunStatus.Success;
                }));
        }

        /// <summary>Apply a debuff to the current target only if not already present (by name).</summary>
        public static Composite Debuff(string name, ReqDelegate req = null)
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
                    return RunStatus.Success;
                }));
        }

        // ---------------- self / friendly buffs ----------------

        /// <summary>Apply a self-buff only if not already active (by aura name).</summary>
        public static Composite BuffSelf(string name, ReqDelegate req = null)
        {
            return BuffUnit(name, ctx => StyxWoW.Me, req);
        }

        /// <summary>Apply a buff to a unit only if that unit does not already have it (by aura name).</summary>
        public static Composite BuffUnit(string name, UnitDelegate onUnit, ReqDelegate req = null)
        {
            return new Decorator(
                ret =>
                {
                    var u = onUnit(ret);
                    if (u == null || u.HasAura(name)) return false;
                    if (req != null && !req(ret)) return false;
                    return CanCast(name, u);
                },
                new Action(ret =>
                {
                    var u = onUnit(ret);
                    if (u == null || !SpellManager.Cast(name, u))
                        return RunStatus.Failure;
                    // Visible (not debug) so buff/seal/blessing upkeep can be confirmed in the log window.
                    Logging.Write("[PallyPilot] " + name + " -> " + u.SafeName());
                    return RunStatus.Success;
                }));
        }
    }

    public delegate int IdDelegate(object ctx);

    internal static class UnitNameExt
    {
        // Some WoWUnit.Name reads can throw briefly during object churn; keep logging safe.
        public static string SafeName(this WoWUnit u)
        {
            try { return u == null ? "<null>" : u.Name; } catch { return "<unit>"; }
        }
    }
}
