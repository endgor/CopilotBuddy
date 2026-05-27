#nullable disable
using System.Collections.Generic;
using System.Linq;
using Styx.Logic.Pathing;

namespace Styx.WoWInternals.World
{
    /// <summary>
    /// Stub WorldScene class. WotLK 3.3.5a has no WorldScene/phased world map system.
    /// Present for API compat with plugins ported from HB 6.2.3+.
    /// </summary>
    public class WorldScene
    {
        private static readonly WorldMap _worldMap = new WorldMap();

        public WorldMap WorldMap => _worldMap;
    }

    /// <summary>
    /// Stub WorldMap. Returns empty map list — WotLK has no phased world maps.
    /// </summary>
    public class WorldMap
    {
        public IEnumerable<JbnMap> GetMaps() => Enumerable.Empty<JbnMap>();

        public JbnMap GetMapAt(WoWPoint location) => null;
    }

    /// <summary>
    /// Stub phased map entry. Represents an active map in the HB 6.2.3+ world scene system.
    /// </summary>
    public class JbnMap
    {
        public bool IsActive => false;
        public int MapID => -1;
    }
}
