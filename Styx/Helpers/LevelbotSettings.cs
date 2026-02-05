using System;
using System.IO;
using System.Reflection;

namespace Styx.Helpers
{
	/// <summary>
	/// LevelbotSettings delegates all UI-visible settings to CharacterSettings.
	/// This ensures that when the code reads LevelbotSettings.Instance.X,
	/// it actually reads CharacterSettings.Instance.X which is bound to the UI.
	/// This pattern matches HonorBuddy 4.3.4 architecture.
	/// </summary>
	public class LevelbotSettings : Settings
	{
		private static string GetSettingsPath()
		{
			string startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
			return Path.Combine(startupPath, string.Format("Settings\\LevelbotSettings_{0}.xml", (StyxWoW.Me != null) ? StyxWoW.Me.Name : ""));
		}

		public LevelbotSettings()
			: base(GetSettingsPath())
		{
		}

		// ===========================================
		// Delegating properties to CharacterSettings
		// (UI binds to CharacterSettings, code uses LevelbotSettings)
		// ===========================================

		public string MailRecipient
		{
			get => CharacterSettings.Instance.MailRecipient;
			set => CharacterSettings.Instance.MailRecipient = value;
		}

		public string FoodName
		{
			get => CharacterSettings.Instance.FoodName;
			set => CharacterSettings.Instance.FoodName = value;
		}

		public string DrinkName
		{
			get => CharacterSettings.Instance.DrinkName;
			set => CharacterSettings.Instance.DrinkName = value;
		}

		public string MountName
		{
			get => CharacterSettings.Instance.MountName;
			set => CharacterSettings.Instance.MountName = value;
		}

		public bool LootMobs
		{
			get => CharacterSettings.Instance.LootMobs;
			set => CharacterSettings.Instance.LootMobs = value;
		}

		public bool SkinMobs
		{
			get => CharacterSettings.Instance.SkinMobs;
			set => CharacterSettings.Instance.SkinMobs = value;
		}

		public bool NinjaSkin
		{
			get => CharacterSettings.Instance.NinjaSkin;
			set => CharacterSettings.Instance.NinjaSkin = value;
		}

		public bool LootChests
		{
			get => CharacterSettings.Instance.LootChests;
			set => CharacterSettings.Instance.LootChests = value;
		}

		public bool HarvestMinerals
		{
			get => CharacterSettings.Instance.HarvestMinerals;
			set => CharacterSettings.Instance.HarvestMinerals = value;
		}

		public bool HarvestHerbs
		{
			get => CharacterSettings.Instance.HarvestHerbs;
			set => CharacterSettings.Instance.HarvestHerbs = value;
		}

		public bool UseMount
		{
			get => CharacterSettings.Instance.UseMount;
			set => CharacterSettings.Instance.UseMount = value;
		}

		public int PullDistance
		{
			get => CharacterSettings.Instance.PullDistance;
			set => CharacterSettings.Instance.PullDistance = value;
		}

		public int MountDistance
		{
			get => CharacterSettings.Instance.MountDistance;
			set => CharacterSettings.Instance.MountDistance = value;
		}

		public int LootRadius
		{
			get => CharacterSettings.Instance.LootRadius;
			set => CharacterSettings.Instance.LootRadius = value;
		}

		public bool FindVendorsAutomatically
		{
			get => CharacterSettings.Instance.FindVendorsAutomatically;
			set => CharacterSettings.Instance.FindVendorsAutomatically = value;
		}

		public bool TrainNewSkills
		{
			get => CharacterSettings.Instance.TrainNewSkills;
			set => CharacterSettings.Instance.TrainNewSkills = value;
		}

		public bool LearnFlightPaths
		{
			get => CharacterSettings.Instance.LearnFlightPaths;
			set => CharacterSettings.Instance.LearnFlightPaths = value;
		}

		public bool UseFlightPaths
		{
			get => CharacterSettings.Instance.UseFlightPaths;
			set => CharacterSettings.Instance.UseFlightPaths = value;
		}

		// ===========================================
		// Local settings (not in UI, stored in LevelbotSettings file)
		// ===========================================

		[DefaultValue(false)]
		[Setting]
		public bool RessAtSpiritHealers { get; set; }

		[DefaultValue(false)]
		[Setting]
		public bool GroundMountFarmingMode { get; set; }

		[Setting]
		[DefaultValue("")]
		public string LastUsedPath { get; set; }

		public static readonly LevelbotSettings Instance = new LevelbotSettings();
	}
}
