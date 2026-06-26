using Styx;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using WarPilot.Core;
using S = WarPilot.Config.WarPilotSettings;

namespace WarPilot.Specs
{
    /// <summary>
    /// Arms warrior — the leveling spec. Battle Stance, single-target priority built to the
    /// verified WotLK 3.3.5a Icy-Veins priority:
    ///   Victory Rush (heal) -> shout/stance upkeep -> Rend -> Overpower (proc) -> Execute
    ///   (&lt;20% or Sudden Death) -> Bladestorm -> Mortal Strike -> Slam filler -> Heroic Strike
    ///   (rage dump) -> face/move to melee.
    ///
    /// Phase 1: single-target only. AoE is a separate, gated placeholder (see settings).
    /// </summary>
    public static class ArmsRotation
    {
        private static LocalPlayer Me { get { return WarCommon.Me; } }
        private static S Cfg { get { return S.Instance; } }

        public static Composite Combat()
        {
            return new PrioritySelector(

                // Interrupt (Pummel) — above EnsureStance so the Berserker dance isn't immediately undone;
                // EnsureStance below restores Battle Stance once the cast is gone.
                new Decorator(ret => Cfg.ArmsUseInterrupts,
                    WarCommon.CreateInterrupt("Pummel", ShapeshiftForm.BerserkerStance, "Berserker Stance")),

                // Stance + auto-attack
                WarCommon.EnsureStance(ShapeshiftForm.BattleStance, "Battle Stance"),
                WarCommon.CreateAutoAttack(),

                // Panic / survival
                WarSpell.BuffSelf("Berserker Rage",
                    ret => Cfg.ArmsUseBerserkerRage && Me.HasAuraWithMechanic(WoWSpellMechanic.Fleeing)),
                WarSpell.Cast("Victory Rush", ret => Cfg.UseVictoryRush),
                WarSpell.BuffSelf("Enraged Regeneration",
                    ret => Cfg.UseEnragedRegen && Me.HealthPercent <= Cfg.EnragedRegenHealth),

                // Rage generation when starved
                WarSpell.BuffSelf("Bloodrage",
                    ret => Cfg.ArmsUseBloodrage && Me.GotTarget && WarCommon.RagePercent < 30),

                // Shout upkeep (one or the other)
                WarCommon.MaintainShout(),

                // Offensive cooldown
                WarSpell.BuffSelf("Recklessness",
                    ret => Cfg.ArmsUseCooldowns && Me.GotTarget && Me.CurrentTarget.HealthPercent >= Cfg.ArmsCooldownMinHealth),

                // AoE — on a pack, add the multi-target-only buttons. Rend / Bladestorm / Mortal Strike /
                // Execute are reached by fall-through to the single-target list below, so they aren't
                // re-listed here (Sweeping Strikes up top makes Mortal Strike below cleave to a 2nd target).
                new Decorator(
                    ret => Cfg.ArmsUseAoE && WarTargeting.NearbyEnemies(10f) >= Cfg.ArmsAoECount,
                    new PrioritySelector(
                        WarSpell.BuffSelf("Sweeping Strikes"),
                        WarSpell.Cast("Thunder Clap"),
                        WarSpell.Cast("Whirlwind"),
                        WarSpell.Cast("Cleave",
                            ret => WarCommon.RagePercent >= Cfg.ArmsHeroicStrikeRage && WarCommon.CanQueueOnSwing))),

                // Rend upkeep — pointless near the Execute window
                WarSpell.Buff("Rend",
                    ret => Cfg.ArmsUseRend && Me.GotTarget && Me.CurrentTarget.HealthPercent > Cfg.ArmsExecuteHealth),

                // Overpower — CanCast is only true while the proc window is open
                WarSpell.Cast("Overpower", ret => Cfg.ArmsUseOverpower),

                // Execute — below execute health % or on a Sudden Death proc
                WarSpell.Cast("Execute",
                    ret => Cfg.ArmsUseExecute && Me.GotTarget &&
                           (Me.CurrentTarget.HealthPercent < Cfg.ArmsExecuteHealth || Me.HasAura("Sudden Death"))),

                // Snare a runner before it pulls adds
                WarSpell.Buff("Hamstring",
                    ret => Cfg.ArmsUseHamstring && Me.GotTarget && Me.CurrentTarget.Fleeing),

                // Bladestorm on cooldown
                WarSpell.Cast("Bladestorm", ret => Cfg.ArmsUseBladestorm),

                // Signature strike
                WarSpell.Cast("Mortal Strike", ret => Cfg.ArmsUseMortalStrike),

                // Filler
                WarSpell.Cast("Slam", ret => Cfg.ArmsUseSlam && WarCommon.RagePercent >= Cfg.ArmsSlamRage),

                // Off-GCD rage dump — gated on rage and not already queued
                WarSpell.Cast("Heroic Strike",
                    ret => WarCommon.RagePercent >= Cfg.ArmsHeroicStrikeRage && WarCommon.CanQueueOnSwing),

                // Ranged poke when out of melee range (closes a gap, or all you can do with movement off)
                WarSpell.Cast("Heroic Throw",
                    ret => Cfg.ArmsUseHeroicThrow && Me.GotTarget && Me.CurrentTarget.Distance > WarMovement.MeleeRange),

                // Movement LAST
                WarMovement.CreateFaceTarget(),
                WarMovement.CreateMoveToMelee()
            );
        }

        public static Composite Pull()
        {
            return new PrioritySelector(

                WarCommon.EnsureStance(ShapeshiftForm.BattleStance, "Battle Stance"),
                WarMovement.CreateFaceTarget(),

                // Charge to open
                WarMovement.CreateCharge(),

                // Heroic Throw to pull a single mob from range when Charge is out of reach
                WarSpell.Cast("Heroic Throw",
                    ret => Me.GotTarget && Me.CurrentTarget.Distance > WarMovement.ChargeMax),

                WarCommon.CreateAutoAttack(),
                WarMovement.CreateMoveToMelee()
            );
        }

        public static Composite PreCombatBuffs()
        {
            return new PrioritySelector(
                WarCommon.EnsureStance(ShapeshiftForm.BattleStance, "Battle Stance"),
                WarCommon.MaintainShout()
            );
        }
    }
}
