using System;

namespace Styx.CommonBot.CharacterManagement
{
    /// <summary>
    /// Character-specific settings for auto-equip and other features
    /// </summary>
    public class CharacterSettings
    {
        private static CharacterSettings instance;

        public static CharacterSettings Instance
        {
            get
            {
                if (instance == null)
                    instance = new CharacterSettings();
                return instance;
            }
        }

        /// <summary>
        /// Enable automatic equipment evaluation for quest rewards
        /// </summary>
        public bool AutoEquip { get; set; } = true;

        private CharacterSettings()
        {
        }
    }
}
