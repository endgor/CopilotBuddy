using System;
using System.IO;
using Styx;
using Styx.Helpers;

namespace Bots.Gather
{
    /// <summary>
    /// Persistent settings for GatherBuddy.
    /// Saved to Settings/GatherBuddySettings_{Name}.xml
    /// Pattern from HB 3.3.5a.
    /// </summary>
    public class GatherBuddySettings : Settings
    {
        public static readonly GatherBuddySettings Instance = new GatherBuddySettings();

        public GatherBuddySettings()
            : base(Path.Combine(Logging.ApplicationPath,
                string.Format("Settings\\GatherBuddySettings_{0}.xml",
                (StyxWoW.Me != null) ? StyxWoW.Me.Name : "")))
        {
        }

        // ═══════════════════════════════════════════════════════════
        // GATHERING
        // ═══════════════════════════════════════════════════════════
        
        [Setting, DefaultValue(true)]
        public bool GatherHerbs { get; set; }

        [Setting, DefaultValue(true)]
        public bool GatherMinerals { get; set; }

        // ═══════════════════════════════════════════════════════════
        // NAVIGATION
        // ═══════════════════════════════════════════════════════════
        
        [Setting, DefaultValue(PathType.Circle)]
        public PathType PathingType { get; set; }
        
        /// <summary>
        /// Maximum detection range for nodes (yards)
        /// </summary>
        [Setting, DefaultValue(70f)]
        public float NodeDetectionRange { get; set; }
        
        /// <summary>
        /// Height modifier for flying (yards above ground)
        /// </summary>
        [Setting, DefaultValue(0f)]
        public float HeightModifier { get; set; }

        // ═══════════════════════════════════════════════════════════
        // COMBAT
        // ═══════════════════════════════════════════════════════════
        
        /// <summary>
        /// Loot killed mobs during gathering
        /// </summary>
        [Setting, DefaultValue(false)]
        public bool LootMobs { get; set; }
        
        /// <summary>
        /// Ignore Elite mobs (do not pull them)
        /// </summary>
        [Setting, DefaultValue(true)]
        public bool IgnoreElites { get; set; }

        /// <summary>
        /// Face nodes before interacting
        /// </summary>
        [Setting, DefaultValue(true)]
        public bool FaceNodes { get; set; }

        // ═══════════════════════════════════════════════════════════
        // ANTI-NINJA
        // ═══════════════════════════════════════════════════════════
        
        /// <summary>
        /// Do not steal nodes from other players
        /// </summary>
        [Setting, DefaultValue(true)]
        public bool NoNinja { get; set; }
        
        /// <summary>
        /// Blacklist duration for failed nodes (seconds)
        /// </summary>
        [Setting, DefaultValue(20)]
        public int BlacklistTimer { get; set; }

        // ═══════════════════════════════════════════════════════════
        // VENDOR/MAIL (Optional - Phase 2)
        // ═══════════════════════════════════════════════════════════
        
        [Setting, DefaultValue(false)]
        public bool MailToAlt { get; set; }
        
        /// <summary>
        /// Mail recipient character name
        /// </summary>
        [Setting, DefaultValue("")]
        public string MailRecipient { get; set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════
        // FLYING — FEAT-40
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Use flying mount + Flightor for navigation when possible.
        /// Requires Cold Weather Flying in Northrend zones.
        /// </summary>
        [Setting, DefaultValue(false)]
        public bool UseFlying { get; set; }

        /// <summary>
        /// Flying altitude above ground (yards). Used as HeightModifier for Flightor.
        /// </summary>
        [Setting, DefaultValue(20f)]
        public float FlyingAltitude { get; set; }

        /// <summary>
        /// Minimum distance to node before descending to gather (yards).
        /// </summary>
        [Setting, DefaultValue(15f)]
        public float FlyingDescentRange { get; set; }

        // ═══════════════════════════════════════════════════════════
        // VENDOR/REPAIR — FEAT-40
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Go to vendor when bags are full.
        /// </summary>
        [Setting, DefaultValue(false)]
        public bool VendorWhenFull { get; set; }

        /// <summary>
        /// Repair gear at vendor when durability is low.
        /// </summary>
        [Setting, DefaultValue(false)]
        public bool RepairAtVendor { get; set; }

        /// <summary>
        /// Durability percentage threshold to trigger repair.
        /// </summary>
        [Setting, DefaultValue(20)]
        public int RepairDurabilityPercent { get; set; }

        /// <summary>
        /// Number of free bag slots below which the bot goes to vendor.
        /// </summary>
        [Setting, DefaultValue(2)]
        public int MinFreeBagSlots { get; set; }
    }
}
