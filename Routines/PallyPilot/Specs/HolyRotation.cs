using System.Collections.Generic;
using Styx;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using PallyPilot.Core;
using S = PallyPilot.Config.PallyPilotSettings;

namespace PallyPilot.Specs
{
    /// <summary>
    /// Holy paladin — dungeon healer (also works solo). Produces the engine's Heal behavior plus a
    /// light self-defence Combat behavior. Built to the verified WotLK 3.3.5a Holy model:
    ///
    ///   * Mana-efficient spell SELECTION by health deficit — Holy Light for heavy damage, the cheap
    ///     fast Flash of Light for moderate damage, instant Holy Shock for emergencies / movement.
    ///   * DOWNRANKING — casts the lowest known rank of Holy Light / Flash of Light that still covers
    ///     the missing health (see PalHeal), so chip damage costs a fraction of the mana.
    ///   * BEACON OF LIGHT kept on the tank so heals on anyone else also land on the tank.
    ///   * SACRED SHIELD kept on the tank (cheapest mitigation).
    ///   * DIVINE FAVOR (guaranteed crit) popped before a big heal on a critical target.
    ///   * Mana cooldowns used SAFELY — Divine Plea only when nobody is in danger (it cuts healing 50%),
    ///     Divine Illumination when mana is low mid-fight, Judgement for mana when nobody needs a heal.
    ///   * CLEANSE of the whole group (Poison/Disease/Magic) with a mana floor.
    /// </summary>
    public static class HolyRotation
    {
        private static LocalPlayer Me { get { return PalCommon.Me; } }
        private static S Cfg { get { return S.Instance; } }

        private static double Deficit(WoWUnit u)
        {
            if (u == null) return 0;
            return u.MaxHealth - u.CurrentHealth;
        }

        // Effective Holy Light threshold: with smart-select off, Holy Light covers the whole heal band.
        private static double HolyLightThreshold
        {
            get { return Cfg.HolySmartSelect ? Cfg.HolyLightHealth : Cfg.HolyFlashHealth; }
        }

        public static Composite Heal()
        {
            return new PrioritySelector(

                // ---- emergency: full heal on a critically low target ----
                PalSpell.CastOn("Lay on Hands", ret => PalTargeting.LowestHealthBelow(Cfg.LayOnHandsHealth),
                    ret => Cfg.UseLayOnHands && PalTargeting.LowestHealthBelow(Cfg.LayOnHandsHealth) != null),

                // Holy Shock — instant; the emergency / on-the-move heal
                PalSpell.CastOn("Holy Shock", ret => PalTargeting.LowestHealthBelow(HolyShockTrigger),
                    ret => Cfg.HolyUseHolyShock
                           && PalTargeting.LowestHealthBelow(HolyShockTrigger) != null
                           && (!Me.IsMoving || Cfg.HolyShockOnMove)),

                // ---- seal / blessing / aura upkeep ----
                // A healer lives in this Heal behavior, so the always-on buffs MUST be maintained here
                // (not just in Combat/PreCombat, which barely run for a non-attacking Holy paladin).
                // Each is a no-op (Failure → fall through) when already up, so this is free on normal ticks.
                PalCommon.MaintainSeal(PalCommon.ResolveHolySeal()),
                PalCommon.MaintainBlessing(),
                PalCommon.MaintainAura(),

                // ---- tank upkeep (instant, high value) ----
                PalSpell.BuffUnit("Beacon of Light", ret => PalTargeting.Tank(),
                    ret => Cfg.HolyUseBeacon && PalTargeting.Tank() != null),
                PalSpell.BuffUnit("Sacred Shield", ret => PalTargeting.Tank(),
                    ret => Cfg.HolyUseSacredShield && PalTargeting.Tank() != null),

                // ---- cleanse the group ----
                PalCommon.CreateCleanse(PalTargeting.FriendlyUnits),

                // ---- guaranteed-crit before a big heal on a critical target ----
                PalSpell.BuffSelf("Divine Favor",
                    ret => Cfg.HolyUseDivineFavor && PalTargeting.LowestHealthBelow(Cfg.HolyDivineFavorHealth) != null),

                // ---- Holy Light (heavy damage), downranked to the deficit ----
                PalSpell.CastIdOn(
                    ret => PalHeal.HolyLightId(Deficit(PalTargeting.LowestHealthBelow(HolyLightThreshold)), Cfg.HolyDownrank),
                    ret => PalTargeting.LowestHealthBelow(HolyLightThreshold),
                    ret => PalTargeting.LowestHealthBelow(HolyLightThreshold) != null),

                // ---- Flash of Light (moderate damage), downranked — only with smart-select ----
                PalSpell.CastIdOn(
                    ret => PalHeal.FlashOfLightId(Deficit(PalTargeting.LowestHealthBelow(Cfg.HolyFlashHealth)), Cfg.HolyDownrank),
                    ret => PalTargeting.LowestHealthBelow(Cfg.HolyFlashHealth),
                    ret => Cfg.HolySmartSelect && PalTargeting.LowestHealthBelow(Cfg.HolyFlashHealth) != null),

                // ---- mana management (only when safe) ----
                PalSpell.BuffSelf("Divine Illumination",
                    ret => Cfg.HolyUseDivineIllumination && Me.Combat && PalCommon.ManaPercent <= Cfg.HolyDivineIlluminationMana),
                PalSpell.BuffSelf("Divine Plea",
                    ret => Cfg.HolyUseDivinePlea
                           && PalCommon.ManaPercent <= Cfg.HolyDivinePleaMana
                           && PalTargeting.LowestHealthBelow(Cfg.HolyDivinePleaSafeHealth) == null),

                // ---- move into range / LoS of whoever needs the next heal ----
                PalMovement.CreateMoveToHealTarget(ret => PalTargeting.LowestHealthBelow(Cfg.HolyStartHealHealth), Cfg.HolyHealRange)
            );
        }

        // Holy Shock should top up anyone moderately hurt (it's our instant), reusing the Flash band.
        private static double HolyShockTrigger { get { return Cfg.HolyFlashHealth; } }

        /// <summary>Light self-defence / upkeep when the botbase has the Holy paladin in combat with an enemy.</summary>
        public static Composite Combat()
        {
            return new PrioritySelector(
                PalCommon.MaintainSeal(PalCommon.ResolveHolySeal()),
                PalCommon.MaintainBlessing(),
                PalCommon.MaintainAura(),
                PalCommon.CreateCleanse(PalTargeting.FriendlyUnits),

                // Judge for mana / Judgements of the Pure when nobody needs a heal.
                PalSpell.Cast("Judgement of Wisdom",
                    ret => Cfg.HolyJudgeForMana && Me.GotTarget
                           && PalTargeting.LowestHealthBelow(Cfg.HolyStartHealHealth) == null),

                // A little damage via Holy Shock when nothing to heal (it damages enemies).
                PalSpell.Cast("Holy Shock",
                    ret => Cfg.HolyUseHolyShock && Me.GotTarget
                           && PalTargeting.LowestHealthBelow(Cfg.HolyStartHealHealth) == null)
            );
        }

        public static Composite PreCombatBuffs()
        {
            return new PrioritySelector(
                PalCommon.MaintainBlessing(),
                PalCommon.MaintainAura(),
                PalCommon.MaintainSeal(PalCommon.ResolveHolySeal()),
                PalSpell.BuffUnit("Beacon of Light", ret => PalTargeting.Tank(),
                    ret => Cfg.HolyUseBeacon && PalTargeting.Tank() != null),
                PalSpell.BuffUnit("Sacred Shield", ret => PalTargeting.Tank(),
                    ret => Cfg.HolyUseSacredShield && PalTargeting.Tank() != null)
            );
        }
    }
}
