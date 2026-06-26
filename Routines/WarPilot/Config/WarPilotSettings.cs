using System.ComponentModel;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace WarPilot.Config
{
    /// <summary>
    /// Per-character persisted settings. Backed by Styx.Helpers.Settings (the same mechanism
    /// Singular uses), saved to Settings\WarPilotSettings_&lt;CharName&gt;.xml.
    ///
    /// NOTE: a [Setting] existing here does NOT mean it is wired into a rotation yet. Each field
    /// is tagged [WIRED] or [PLACEHOLDER] in its comment so the UI can grey-out the unwired ones.
    /// Phase 1 wires the General + Arms single-target core; everything else is a placeholder.
    /// </summary>
    public class WarPilotSettings : Settings
    {
        private static WarPilotSettings _instance;
        public static WarPilotSettings Instance
        {
            get { return _instance ?? (_instance = new WarPilotSettings()); }
        }

        public static string Path
        {
            get { return string.Format("{0}\\Settings\\WarPilotSettings_{1}", Logging.ApplicationPath, StyxWoW.Me.Name); }
        }

        public WarPilotSettings() : base(Path + ".xml") { }

        // ---------------- General (WIRED) ----------------

        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Keep proper stance")]
        [Description("Switch to the correct stance for the active spec (Arms: Battle, Prot: Defensive).")]
        public bool KeepStance { get; set; }

        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Enable movement")]
        [Description("Let the routine path/move into melee range. Turn OFF to position the character yourself (the rotation still fires, but won't move you to the target).")]
        public bool EnableMovement { get; set; }

        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Enable targeting")]
        [Description("Let the routine pick a target when you have none (nearest enemy attacking you). Turn OFF to only ever act on the target you/the botbase selected.")]
        public bool EnableTargeting { get; set; }

        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Use Victory Rush")]
        [Description("Fire Victory Rush after a kill for free self-healing while leveling.")]
        public bool UseVictoryRush { get; set; }

        [Setting, DefaultValue(false)]
        [Category("General"), DisplayName("Use Commanding Shout (else Battle Shout)")]
        [Description("Keep Commanding Shout (max health) up instead of Battle Shout (attack power).")]
        public bool UseCommandingShout { get; set; }

        [Setting, DefaultValue(true)]
        [Category("General"), DisplayName("Use Enraged Regeneration")]
        [Description("Use Enraged Regeneration as an emergency self-heal below the health threshold.")]
        public bool UseEnragedRegen { get; set; }

        [Setting, DefaultValue(40)]
        [Category("General"), DisplayName("Enraged Regeneration health %")]
        [Description("Health percent below which Enraged Regeneration is used.")]
        public int EnragedRegenHealth { get; set; }

        // ---------------- General (PLACEHOLDER — not wired yet) ----------------

        [Setting, DefaultValue(false)]
        [Category("General"), DisplayName("Use racials (PLACEHOLDER)")]
        [Description("PLACEHOLDER — not wired yet. Will use Blood Fury / Berserking / etc.")]
        public bool UseRacials { get; set; }

        [Setting, DefaultValue(false)]
        [Category("General"), DisplayName("Use trinkets (PLACEHOLDER)")]
        [Description("PLACEHOLDER — not wired yet. Will activate on-use trinkets with damage cooldowns.")]
        public bool UseTrinkets { get; set; }

        // ---------------- Arms (WIRED) ----------------

        // -- core strikes --
        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Keep Rend up")]
        [Description("Maintain Rend on the target (feeds Taste for Blood / Overpower).")]
        public bool ArmsUseRend { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use Overpower")]
        [Description("Use Overpower on proc (dodge / Taste for Blood).")]
        public bool ArmsUseOverpower { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use Mortal Strike")]
        [Description("Use Mortal Strike on cooldown (the signature Arms strike + healing-reduction debuff).")]
        public bool ArmsUseMortalStrike { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use Bladestorm")]
        [Description("Use Bladestorm on cooldown.")]
        public bool ArmsUseBladestorm { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use Slam filler")]
        [Description("Weave Slam as filler when rage allows (best with Improved Slam).")]
        public bool ArmsUseSlam { get; set; }

        [Setting, DefaultValue(40)]
        [Category("Arms"), DisplayName("Slam minimum rage %")]
        [Description("Only weave Slam when rage is at/above this percent.")]
        public int ArmsSlamRage { get; set; }

        // -- execute / finishers --
        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use Execute")]
        [Description("Use Execute below the execute health % or on a Sudden Death proc.")]
        public bool ArmsUseExecute { get; set; }

        [Setting, DefaultValue(20)]
        [Category("Arms"), DisplayName("Execute health %")]
        [Description("Target health percent below which Execute is used.")]
        public int ArmsExecuteHealth { get; set; }

        // -- rage dump / ranged --
        [Setting, DefaultValue(50)]
        [Category("Arms"), DisplayName("Heroic Strike rage dump %")]
        [Description("Queue Heroic Strike only when rage is at/above this percent so it never delays core abilities.")]
        public int ArmsHeroicStrikeRage { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use Heroic Throw")]
        [Description("Throw Heroic Throw at targets out of melee range (handy ranged poke, especially with movement disabled).")]
        public bool ArmsUseHeroicThrow { get; set; }

        // -- utility --
        [Setting, DefaultValue(false)]
        [Category("Arms"), DisplayName("Use Hamstring on fleeing targets")]
        [Description("Apply Hamstring to a fleeing target to stop runners pulling extra mobs.")]
        public bool ArmsUseHamstring { get; set; }

        // -- rage / cooldowns --
        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use Bloodrage")]
        [Description("Use Bloodrage to generate rage when low (small self-damage).")]
        public bool ArmsUseBloodrage { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use Berserker Rage (break fear)")]
        [Description("Use Berserker Rage to break fear effects (and generate rage).")]
        public bool ArmsUseBerserkerRage { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use damage cooldowns (Recklessness)")]
        [Description("Use Recklessness on targets at/above the cooldown health %.")]
        public bool ArmsUseCooldowns { get; set; }

        [Setting, DefaultValue(50)]
        [Category("Arms"), DisplayName("Recklessness min target health %")]
        [Description("Only use Recklessness when the target's health is at/above this percent (saves it for worthwhile fights).")]
        public int ArmsCooldownMinHealth { get; set; }

        // ---------------- Arms — AoE & interrupts (WIRED) ----------------

        [Setting, DefaultValue(true)]
        [Category("Arms"), DisplayName("Use AoE rotation on packs")]
        [Description("At 'AoE enemy count' or more nearby enemies: Sweeping Strikes + Bladestorm + Thunder Clap + Whirlwind + Cleave.")]
        public bool ArmsUseAoE { get; set; }

        [Setting, DefaultValue(3)]
        [Category("Arms"), DisplayName("AoE enemy count")]
        [Description("Minimum nearby enemies (within ~10 yds, engaged with you) before the AoE rotation kicks in.")]
        public int ArmsAoECount { get; set; }

        [Setting, DefaultValue(false)]
        [Category("Arms"), DisplayName("Interrupt casts (Pummel)")]
        [Description("Interrupt an enemy's interruptible cast with Pummel. This stance-dances to Berserker and back, briefly leaving Battle Stance.")]
        public bool ArmsUseInterrupts { get; set; }

        // ---------------- Protection (PLACEHOLDER — Phase 2) ----------------

        [Setting, DefaultValue(true)]
        [Category("Protection"), DisplayName("Auto-taunt off party members (PLACEHOLDER)")]
        [Description("PLACEHOLDER — Phase 2. Taunt mobs that target a party member.")]
        public bool ProtAutoTaunt { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Protection"), DisplayName("969 threat rotation (PLACEHOLDER)")]
        [Description("PLACEHOLDER — Phase 2. Shield Slam / Revenge / Devastate priority.")]
        public bool ProtUse969 { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Protection"), DisplayName("Use defensive cooldowns (PLACEHOLDER)")]
        [Description("PLACEHOLDER — Phase 2. Shield Block / Last Stand / Shield Wall.")]
        public bool ProtUseDefensives { get; set; }
    }
}
