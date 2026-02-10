using System;

namespace Styx.WoWInternals.WoWObjects
{
    /// <summary>
    /// Represents a fishing bobber in WoW.
    /// WoW 3.3.5a build 12340.
    /// </summary>
    public class WoWFishingBobber : WoWAnimatedSubObject
    {
        internal WoWFishingBobber(uint baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Indicates whether the bobber is moving (fish biting).
        /// AnimationState == 8 means the fish has bitten.
        /// </summary>
        public bool IsBobbing => AnimationState == 8;
    }
}
