using System;
using GreenMagic;

namespace Styx.WoWInternals.WoWObjects
{
    /// <summary>
    /// Represents an animated WoW sub-object.
    /// WoW 3.3.5a build 12340.
    /// </summary>
    public class WoWAnimatedSubObject : WoWSubObject
    {
        internal WoWAnimatedSubObject(uint baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Current animation state of the sub-object.
        /// Offset +16 (0x10).
        /// </summary>
        public int AnimationState
        {
            get
            {
                Memory? wow = ObjectManager.Wow;
                if (wow == null) return 0;
                return wow.Read<int>(BaseAddress + 16);
            }
        }
    }
}
