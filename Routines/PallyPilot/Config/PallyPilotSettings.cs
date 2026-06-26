using System.ComponentModel;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace PallyPilot.Config
{
    // ---- choice enums (persisted by name in the per-character settings XML) ----

    /// <summary>Which Blessing to keep on self. Auto = Kings if known, else Might (user policy).</summary>
    public enum BlessingChoice { Auto, Might, Kings, Wisdom, Sanctuary }

    /// <summary>Which Aura to keep up. Auto = Retribution(Ret) / Concentration(Holy) / Devotion(else).</summary>
    public enum AuraChoice { Auto, Devotion, Retribution, Concentration }

    /// <summary>Which Seal to keep up in Retribution. Auto = Command if known, else Righteousness.</summary>
    public enum RetSealChoice { Auto, Command, Righteousness, VengeanceCorruption, Wisdom, Light, Martyr }

    /// <summary>Which Judgement to cast (also unleashes the active seal).</summary>
    public enum JudgementChoice { Wisdom, Light, Justice }

    /// <summary>
    /// Per-character persisted settings for PallyPilot, backed by Styx.Helpers.Settings (the same
    /// mechanism Singular/WarPilot use). Saved to Settings\PallyPilotSettings_&lt;CharName&gt;.xml.
    ///
    /// Focus is Retribution (leveling) and Holy (dungeon healing) — both fully wired. Protection is a
    /// minimal fallback stub. Every field is tagged [WIRED] or [PLACEHOLDER] so the GUI can grey-out
    /// the unwired ones.
    /// </summary>
    public class PallyPilotSettings : Settings
    {
        private static PallyPilotSettings _instance;
        public static PallyPilotSettings Instance
        {
            get { return _instance ?? (_instance = new PallyPilotSettings()); }
        }

        public static string Path
        {
            get { return string.Format("{0}\\Settings\\PallyPilotSettings_{1}", Logging.ApplicationPath, StyxWoW.Me.Name); }
        }

        public PallyPilotSettings() : base(Path + ".xml") { }

        // ===================== General (WIRED) =====================

        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Enable movement")]
        [Description("Let the routine path/move into melee (Ret) or into heal range (Holy). Turn OFF to position yourself; the rotation still fires but won't move you.")]
        public bool EnableMovement { get; set; }

        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Enable targeting (Ret)")]
        [Description("Ret only: pick a target when you have none (nearest enemy attacking you). Holy never auto-targets enemies.")]
        public bool EnableTargeting { get; set; }

        // -- blessings --
        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Keep a Blessing up")]
        [Description("Maintain a self Blessing. Auto picks Kings if you have it, otherwise Might.")]
        public bool KeepBlessing { get; set; }

        [Setting, DefaultValue(BlessingChoice.Auto)]
        [Category("General"), DisplayName("Blessing")]
        [Description("Which Blessing to keep on yourself. Auto = Kings if known, else Might.")]
        public BlessingChoice Blessing { get; set; }

        // -- auras --
        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Keep an Aura up")]
        [Description("Maintain a Paladin aura. Auto = Retribution Aura (Ret), Concentration Aura (Holy), else Devotion Aura.")]
        public bool KeepAura { get; set; }

        [Setting, DefaultValue(AuraChoice.Auto)]
        [Category("General"), DisplayName("Aura")]
        [Description("Which aura to keep up. Auto adapts to the active spec.")]
        public AuraChoice Aura { get; set; }

        // -- cleanse (both specs) --
        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Auto Cleanse / Purify")]
        [Description("Remove harmful Poison/Disease (and Magic with Cleanse) from yourself and group members. Cleanse used if known, else Purify.")]
        public bool AutoCleanse { get; set; }

        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Cleanse Magic too")]
        [Description("Also dispel Magic effects (Cleanse only). Disable to save mana / avoid stripping beneficial-looking debuffs.")]
        public bool CleanseMagic { get; set; }

        [Setting, DefaultValue(20)]
        [Category("General"), DisplayName("Cleanse minimum mana %")]
        [Description("Don't cleanse below this mana percent (mana safety).")]
        public int CleanseMinMana { get; set; }

        // -- emergency --
        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Use Lay on Hands (emergency)")]
        [Description("Full heal on a critically low target (self or tank) when off cooldown.")]
        public bool UseLayOnHands { get; set; }

        [Setting, DefaultValue(20)]
        [Category("General"), DisplayName("Lay on Hands health %")]
        [Description("Health percent below which Lay on Hands fires. The emergency full heal — works even through Forbearance, so it's the real panic button (20 min cooldown).")]
        public int LayOnHandsHealth { get; set; }

        // ===================== General (PLACEHOLDER) =====================

        [Setting, DefaultValue(false)]
        [Category("General"), DisplayName("Use racials (PLACEHOLDER)")]
        [Description("PLACEHOLDER — not wired yet.")]
        public bool UseRacials { get; set; }

        [Setting, DefaultValue(false)]
        [Category("General"), DisplayName("Use trinkets (PLACEHOLDER)")]
        [Description("PLACEHOLDER — not wired yet.")]
        public bool UseTrinkets { get; set; }

        // ===================== Retribution (WIRED) =====================

        [Setting, DefaultValue(RetSealChoice.Auto)]
        [Category("Retribution"), DisplayName("Seal")]
        [Description("Which seal to keep up. Auto = Command if known, else Righteousness. Vengeance/Corruption auto-picks the faction version.")]
        public RetSealChoice RetSeal { get; set; }

        [Setting, DefaultValue(JudgementChoice.Wisdom)]
        [Category("Retribution"), DisplayName("Judgement")]
        [Description("Which Judgement to cast on cooldown (also unleashes the seal). Wisdom returns mana to the party.")]
        public JudgementChoice RetJudgement { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Crusader Strike")]
        [Description("Core single-target strike on cooldown.")]
        public bool RetUseCrusaderStrike { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Divine Storm")]
        [Description("Use Divine Storm on cooldown (cleaves + self-heals).")]
        public bool RetUseDivineStorm { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Judgement")]
        [Description("Judge on cooldown.")]
        public bool RetUseJudgement { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Consecration")]
        [Description("Use Consecration (ground AoE). Gated by the Consecration mana floor to avoid draining mana solo.")]
        public bool RetUseConsecration { get; set; }

        [Setting, DefaultValue(40)]
        [Category("Retribution"), DisplayName("Consecration mana floor %")]
        [Description("Only Consecrate when mana is at/above this percent (Consecration is mana-hungry).")]
        public int RetConsecrationMana { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Exorcism (Art of War)")]
        [Description("Fire Exorcism only when it is an instant free cast from the Art of War proc.")]
        public bool RetUseExorcism { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Holy Wrath (vs Undead/Demon)")]
        [Description("Use Holy Wrath when fighting Undead or Demons.")]
        public bool RetUseHolyWrath { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Hammer of Wrath (execute)")]
        [Description("Use Hammer of Wrath as an execute below the health threshold.")]
        public bool RetUseHammerOfWrath { get; set; }

        [Setting, DefaultValue(20)]
        [Category("Retribution"), DisplayName("Hammer of Wrath health %")]
        [Description("Target health percent below which Hammer of Wrath is usable (20% in WotLK).")]
        public int RetHammerOfWrathHealth { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Avenging Wrath")]
        [Description("Use Avenging Wrath (damage cooldown) when engaging a worthwhile target.")]
        public bool RetUseAvengingWrath { get; set; }

        [Setting, DefaultValue(70)]
        [Category("Retribution"), DisplayName("Avenging Wrath min target health %")]
        [Description("Only pop Avenging Wrath when the target is at/above this health (saves it for real fights).")]
        public int RetAvengingWrathHealth { get; set; }

        // -- AoE --
        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use AoE rotation on packs")]
        [Description("On 'AoE enemy count'+ nearby enemies, prioritise Consecration + Divine Storm + Holy Wrath.")]
        public bool RetUseAoE { get; set; }

        [Setting, DefaultValue(3)]
        [Category("Retribution"), DisplayName("AoE enemy count")]
        [Description("Minimum nearby engaged enemies before the AoE priority kicks in.")]
        public int RetAoECount { get; set; }

        // -- self-sustain / defensives --
        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Self-heal with Flash of Light")]
        [Description("While leveling, cast Flash of Light on yourself when low (costs mana, keeps you alive solo).")]
        public bool RetSelfHeal { get; set; }

        [Setting, DefaultValue(50)]
        [Category("Retribution"), DisplayName("Self-heal health %")]
        [Description("Health percent below which the Ret self-heal fires.")]
        public int RetSelfHealHealth { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Divine Protection")]
        [Description("Use Divine Protection (damage reduction) when low.")]
        public bool RetUseDivineProtection { get; set; }

        [Setting, DefaultValue(35)]
        [Category("Retribution"), DisplayName("Divine Protection health %")]
        [Description("Health percent below which Divine Protection is used.")]
        public int RetDivineProtectionHealth { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Divine Shield (panic)")]
        [Description("Use Divine Shield (bubble) as a last resort when critically low and Forbearance is not active.")]
        public bool RetUseDivineShield { get; set; }

        [Setting, DefaultValue(12)]
        [Category("Retribution"), DisplayName("Divine Shield health %")]
        [Description("Health percent below which Divine Shield (immunity) is used as the absolute last resort. Blocked if Forbearance is up (e.g. after Divine Protection), so it's the fallback when Lay on Hands is on cooldown.")]
        public int RetDivineShieldHealth { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Retribution"), DisplayName("Use Divine Plea for mana")]
        [Description("Use Divine Plea to refill mana when low (Ret takes no real healing penalty mid-grind).")]
        public bool RetUseDivinePlea { get; set; }

        [Setting, DefaultValue(40)]
        [Category("Retribution"), DisplayName("Divine Plea mana %")]
        [Description("Mana percent below which Divine Plea is used.")]
        public int RetDivinePleaMana { get; set; }

        // ===================== Holy (WIRED) =====================

        [Setting, DefaultValue(40)]
        [Category("Holy"), DisplayName("Heal range (yards)")]
        [Description("Only consider group members within this range as heal targets.")]
        public int HolyHealRange { get; set; }

        [Setting, DefaultValue(90)]
        [Category("Holy"), DisplayName("Start healing below health %")]
        [Description("Don't bother healing anyone above this health (mana saving — no chip-heal waste).")]
        public int HolyStartHealHealth { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Smart spell selection")]
        [Description("Pick Flash of Light vs Holy Light vs Holy Shock by the size of the health deficit (mana-efficient).")]
        public bool HolySmartSelect { get; set; }

        [Setting, DefaultValue(75)]
        [Category("Holy"), DisplayName("Flash of Light below health %")]
        [Description("Use the fast cheap Flash of Light for moderate damage (above the Holy Light threshold).")]
        public int HolyFlashHealth { get; set; }

        [Setting, DefaultValue(55)]
        [Category("Holy"), DisplayName("Holy Light below health %")]
        [Description("Use the big Holy Light when a target drops below this (heavy damage).")]
        public int HolyLightHealth { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Use spell downranking")]
        [Description("Cast the lowest rank of Holy Light / Flash of Light that still covers the missing health — large mana savings on light damage.")]
        public bool HolyDownrank { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Use Holy Shock")]
        [Description("Instant Holy Shock for movement healing and quick top-ups (on cooldown).")]
        public bool HolyUseHolyShock { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Holy Shock when moving")]
        [Description("Allow Holy Shock to heal while you are moving (it is instant).")]
        public bool HolyShockOnMove { get; set; }

        // -- beacon --
        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Keep Beacon of Light on tank")]
        [Description("Maintain Beacon of Light on the party/raid tank so heals on others also land on the tank.")]
        public bool HolyUseBeacon { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Beacon self if no tank")]
        [Description("If no tank is found (e.g. solo / leveling Holy), put Beacon on yourself.")]
        public bool HolyBeaconSelfIfSolo { get; set; }

        // -- sacred shield --
        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Keep Sacred Shield on tank")]
        [Description("Maintain Sacred Shield (absorb) on the tank — the most mana-efficient mitigation.")]
        public bool HolyUseSacredShield { get; set; }

        // -- divine favor --
        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Use Divine Favor on critical target")]
        [Description("Pop Divine Favor (guaranteed crit) before a big heal when a target is critically low.")]
        public bool HolyUseDivineFavor { get; set; }

        [Setting, DefaultValue(40)]
        [Category("Holy"), DisplayName("Divine Favor health %")]
        [Description("Target health percent below which Divine Favor is used.")]
        public int HolyDivineFavorHealth { get; set; }

        // -- mana management --
        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Use Divine Plea (safe windows only)")]
        [Description("Use Divine Plea for mana — but ONLY when nobody is in danger, since it reduces your healing 50% while active.")]
        public bool HolyUseDivinePlea { get; set; }

        [Setting, DefaultValue(60)]
        [Category("Holy"), DisplayName("Divine Plea mana %")]
        [Description("Mana percent below which Divine Plea is used.")]
        public int HolyDivinePleaMana { get; set; }

        [Setting, DefaultValue(85)]
        [Category("Holy"), DisplayName("Divine Plea safe health %")]
        [Description("Only Divine Plea when the lowest member is at/above this health (so the -50% penalty is harmless).")]
        public int HolyDivinePleaSafeHealth { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Use Divine Illumination")]
        [Description("Use Divine Illumination (-50% spell cost) during heavy healing when mana is low.")]
        public bool HolyUseDivineIllumination { get; set; }

        [Setting, DefaultValue(35)]
        [Category("Holy"), DisplayName("Divine Illumination mana %")]
        [Description("Mana percent below which Divine Illumination is used during combat.")]
        public int HolyDivineIlluminationMana { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Holy"), DisplayName("Judge for mana when safe")]
        [Description("When an enemy is in range and nobody needs healing, Judge to keep Judgements of the Pure up and return mana.")]
        public bool HolyJudgeForMana { get; set; }

        // ===================== Protection (PLACEHOLDER — stub spec) =====================

        [Setting, DefaultValue(true)]
        [Category("Protection"), DisplayName("Basic threat fallback (PLACEHOLDER)")]
        [Description("PLACEHOLDER — Protection is a minimal fallback (Righteous Fury + basic attacks). Full tanking is not implemented.")]
        public bool ProtBasic { get; set; }
    }
}
