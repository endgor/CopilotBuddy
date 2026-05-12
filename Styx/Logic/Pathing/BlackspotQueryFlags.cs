using System;

namespace Styx.Logic.Pathing
{
    /// <summary>
    /// Flags that control which categories of blackspot are queried.
    /// Port of HB 6.2.3 Styx.Pathing.BlackspotQueryFlags.
    /// </summary>
    [Flags]
    public enum BlackspotQueryFlags : uint
    {
        Static  = 1U,
        Dynamic = 2U,
        All     = 4294967295U
    }
}
