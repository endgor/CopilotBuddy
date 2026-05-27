using Styx.Logic.Inventory.Frames.Merchant;

namespace Styx.Helpers
{
    /// <summary>
    /// HB 6.2.3: Converts WoWItemQuality to the ItemQuality flags enum used by merchant/vendor logic.
    /// WotLK: Artifact maps to Legendary (same int value 32 in CB enum).
    /// </summary>
    public static class WoWItemQualityExtensions
    {
        public static ItemQuality ToFlag(this WoWItemQuality quality)
        {
            switch (quality)
            {
                case WoWItemQuality.Poor:      return ItemQuality.Poor;
                case WoWItemQuality.Common:    return ItemQuality.Common;
                case WoWItemQuality.Uncommon:  return ItemQuality.Uncommon;
                case WoWItemQuality.Rare:      return ItemQuality.Rare;
                case WoWItemQuality.Epic:      return ItemQuality.Epic;
                case WoWItemQuality.Legendary: return ItemQuality.Legendary;
                case WoWItemQuality.Artifact:  return ItemQuality.Legendary;
                case WoWItemQuality.Heirloom:  return ItemQuality.Heirloom;
                default:                       return ItemQuality.None;
            }
        }
    }
}
