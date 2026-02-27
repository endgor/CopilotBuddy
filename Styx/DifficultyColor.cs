using System;

namespace Styx
{
    /// <summary>
    /// Represents the color-coded difficulty of a unit relative to the player.
    /// Copied from HonorBuddy 3.3.5a/5.4.8/6.2.3 and used by targeting and other logic.
    /// </summary>
    public enum DifficultyColor
    {
        Gray,
        Green,
        Yellow,
        Orange,
        Red,
        Skull
    }
}