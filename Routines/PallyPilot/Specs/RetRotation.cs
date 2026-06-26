using System.Collections.Generic;
using Styx;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using PallyPilot.Core;
using S = PallyPilot.Config.PallyPilotSettings;

namespace PallyPilot.Specs
{
    /// <summary>
    /// Retribution paladin — the leveling / questing / dungeon-DPS spec. Built to the verified WotLK
    /// 3.3.5a Icy-Veins priority, with smart mana use (Consecration gated by a mana floor, Exorcism
    /// only on the free Art of War proc) and full self-sustain so it survives solo grinding.
    ///
    /// Priority: panic defensives/self-heal -> cleanse self -> seal/blessing/aura upkeep ->
    /// Avenging Wrath -> Divine Plea (mana) -> [AoE: Consecration/Divine Storm/Holy Wrath] ->
    /// Hammer of Wrath (execute) -> Exorcism (Art of War) -> Judgement -> Crusader Strike ->
    /// Divine Storm -> Consecration -> Holy Wrath (vs undead/demon) -> face/move to melee.
    /// </summary>
    public static class RetRotation
    {
        private static LocalPlayer Me { get { return PalCommon.Me; } }
        private static S Cfg { get { return S.Instance; } }

        private static bool TargetIsUndeadOrDemon
        {
            get
            {
                var t = Me.CurrentTarget;
                return t != null && (t.CreatureType == WoWCreatureType.Undead || t.CreatureType == WoWCreatureType.Demon);
            }
        }

        private static IEnumerable<WoWUnit> SelfOnly() { yield return StyxWoW.Me; }

        public static Composite Combat()
        {
            return new PrioritySelector(

                // ---- get swinging (white damage + seal procs; never blocks the rotation) ----
                PalCommon.CreateAutoAttack(),

                // ---- survival ladder (WotLK 3.3.5a Forbearance-aware) ----
                // Forbearance (2 min) is caused by AND blocks both Divine Shield and Divine Protection,
                // so those two lock each other out. Lay on Hands is the exception: it is NOT blocked by
                // Forbearance, so it still works after a bubble — that's why it's the true panic full-heal.
                //
                // As health falls the thresholds fire in this order: Holy Light heal (<=50%) keeps you up;
                // Divine Protection (<=35%) is the proactive 50%-mitigation layer; Lay on Hands (<=20%)
                // is the emergency full heal (works through Forbearance); Divine Shield (<=12%) is the
                // last-ditch immunity if Lay on Hands is on its 20-min cooldown. When several are true at
                // once (deep critical), the list order makes the full heal win over the bubbles.

                // Emergency full heal — highest value, works even if Forbearance is up. (CanCast gates the 20m CD.)
                PalSpell.CastOn("Lay on Hands", ret => StyxWoW.Me,
                    ret => Cfg.UseLayOnHands && Me.HealthPercent <= Cfg.LayOnHandsHealth),

                // Last-ditch immunity when Lay on Hands is unavailable and Forbearance isn't up.
                PalSpell.BuffSelf("Divine Shield",
                    ret => Cfg.RetUseDivineShield && Me.HealthPercent <= Cfg.RetDivineShieldHealth && !Me.HasAura("Forbearance")),

                // Proactive damage mitigation (causes/needs no Forbearance to fire).
                PalSpell.BuffSelf("Divine Protection",
                    ret => Cfg.RetUseDivineProtection && Me.HealthPercent <= Cfg.RetDivineProtectionHealth && !Me.HasAura("Forbearance")),

                // Self-heal with the best heal we actually KNOW: Flash of Light (fast/cheap, lvl 20+) if
                // learned, otherwise Holy Light (the level-1 heal). Without this fallback a low-level Ret
                // never heals — it only knows Holy Light, and the old code only tried Flash of Light.
                new Decorator(
                    ret => Cfg.RetSelfHeal && Me.HealthPercent <= Cfg.RetSelfHealHealth,
                    new PrioritySelector(
                        PalSpell.CastOn("Flash of Light", ret => StyxWoW.Me),
                        PalSpell.CastOn("Holy Light", ret => StyxWoW.Me))),

                // ---- cleanse self (Poison/Disease/Magic) ----
                PalCommon.CreateCleanse(SelfOnly),

                // ---- upkeep ----
                PalCommon.MaintainSeal(PalCommon.ResolveRetSeal()),
                PalCommon.MaintainBlessing(),
                PalCommon.MaintainAura(),

                // ---- offensive cooldown + mana ----
                PalSpell.BuffSelf("Avenging Wrath",
                    ret => Cfg.RetUseAvengingWrath && Me.GotTarget && Me.CurrentTarget.HealthPercent >= Cfg.RetAvengingWrathHealth),
                PalSpell.BuffSelf("Divine Plea",
                    ret => Cfg.RetUseDivinePlea && PalCommon.ManaPercent <= Cfg.RetDivinePleaMana),

                // ---- AoE block: only the multi-target-exclusive buttons (rest fall through) ----
                new Decorator(
                    ret => Cfg.RetUseAoE && PalTargeting.NearbyEnemies(10f) >= Cfg.RetAoECount,
                    new PrioritySelector(
                        PalSpell.Cast("Consecration", ret => PalCommon.ManaPercent >= Cfg.RetConsecrationMana),
                        PalSpell.Cast("Divine Storm", ret => Cfg.RetUseDivineStorm),
                        PalSpell.Cast("Holy Wrath", ret => Cfg.RetUseHolyWrath && TargetIsUndeadOrDemon))),

                // ---- single-target priority ----
                PalSpell.Cast("Hammer of Wrath",
                    ret => Cfg.RetUseHammerOfWrath && Me.GotTarget && Me.CurrentTarget.HealthPercent <= Cfg.RetHammerOfWrathHealth),

                // Exorcism is only worth a GCD when Art of War makes it instant + free
                PalSpell.Cast("Exorcism",
                    ret => Cfg.RetUseExorcism && Me.HasAura("The Art of War")),

                PalSpell.Cast(PalCommon.ResolveJudgement(), ret => Cfg.RetUseJudgement),
                PalSpell.Cast("Crusader Strike", ret => Cfg.RetUseCrusaderStrike),
                PalSpell.Cast("Divine Storm", ret => Cfg.RetUseDivineStorm),
                PalSpell.Cast("Consecration",
                    ret => Cfg.RetUseConsecration && PalCommon.ManaPercent >= Cfg.RetConsecrationMana),
                PalSpell.Cast("Holy Wrath", ret => Cfg.RetUseHolyWrath && TargetIsUndeadOrDemon),

                // ---- movement LAST ----
                PalMovement.CreateFaceTarget(),
                PalMovement.CreateMoveToMelee()
            );
        }

        public static Composite Pull()
        {
            return new PrioritySelector(
                PalCommon.MaintainSeal(PalCommon.ResolveRetSeal()),
                PalCommon.CreateAutoAttack(),
                PalMovement.CreateFaceTarget(),
                // Judgement is a fine ranged opener; otherwise close the gap.
                PalSpell.Cast(PalCommon.ResolveJudgement(),
                    ret => Cfg.RetUseJudgement && Me.GotTarget && Me.CurrentTarget.Distance > PalMovement.MeleeRange),
                PalSpell.Cast("Exorcism", ret => Cfg.RetUseExorcism && Me.HasAura("The Art of War")),
                PalMovement.CreateMoveToMelee()
            );
        }

        public static Composite PreCombatBuffs()
        {
            return new PrioritySelector(
                PalCommon.MaintainBlessing(),
                PalCommon.MaintainAura(),
                PalCommon.MaintainSeal(PalCommon.ResolveRetSeal())
            );
        }
    }
}
