using System;

namespace Tripper.Navigation
{
    /// <summary>
    /// Event arguments for the <see cref="Navigator.MapLoaded"/> event.
    /// </summary>
    public class MapLoadedEventArgs : EventArgs
    {
        public MapLoadedEventArgs(uint mapId)
        {
            MapId = mapId;
        }

        public uint MapId { get; }
    }
}
