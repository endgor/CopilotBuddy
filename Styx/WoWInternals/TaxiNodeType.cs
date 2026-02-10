using System;

namespace Styx.WoWInternals
{
    /// <summary>
    /// Taxi node type (flight points).
    /// WoW 3.3.5a build 12340.
    /// </summary>
    public enum TaxiNodeType
    {
        /// <summary>Current player position</summary>
        Current = 0,
        
        /// <summary>Reachable node</summary>
        Reachable = 1,
        
        /// <summary>Distant node (not yet discovered)</summary>
        Distant = 2,
        
        /// <summary>No node</summary>
        None = 3
    }
}
