#nullable disable
using Styx.WoWInternals.WoWObjects;

namespace Styx.CommonBot.CharacterManagement
{
    /// <summary>
    /// Represents a quest reward choice item
    /// </summary>
    public class QuestRewardItem
    {
        /// <summary>
        /// Index of the reward (0-based)
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Item link from game
        /// </summary>
        public string ItemLink { get; set; }

        /// <summary>
        /// Parsed item info
        /// </summary>
        public ItemInfo ItemInfo { get; set; }

        /// <summary>
        /// Number of items in stack
        /// </summary>
        public int Count { get; set; }
    }
}
