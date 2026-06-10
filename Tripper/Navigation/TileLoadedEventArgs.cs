using System;

namespace Tripper.Navigation
{
    /// <summary>
    /// Event arguments for the <see cref="Navigator.TileLoaded"/> event.
    /// 1x1 MaNGOS-style reading: one event per ADT tile loaded by Navigation.dll.
    /// </summary>
    public class TileLoadedEventArgs : EventArgs
    {
        public TileLoadedEventArgs(uint mapId, int tileX, int tileY)
        {
            MapId = mapId;
            TileX = tileX;
            TileY = tileY;
        }

        public uint MapId { get; }
        public int TileX { get; }
        public int TileY { get; }
    }
}
