#nullable disable
namespace Styx.CommonBot.CharacterManagement
{
    /// <summary>
    /// Central manager for character-specific functionality
    /// </summary>
    public static class CharacterManager
    {
        private static AutoEquipper autoEquipper;

        /// <summary>
        /// Gets the automatic equipment evaluator instance
        /// </summary>
        public static AutoEquipper AutoEquip
        {
            get
            {
                if (autoEquipper == null)
                    autoEquipper = new AutoEquipper();
                return autoEquipper;
            }
        }
    }
}
