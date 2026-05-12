using System;

namespace Tripper.Navigation
{
    // HB 6.2.3 Tripper/Navigation/TileLoadedEventArgs.cs
    public class TileLoadedEventArgs : EventArgs
    {
        public TileLoadedEventArgs()
        {
        }

        public TileLoadedEventArgs(uint mapId, int tileX, int tileY)
        {
            MapId = mapId;
            Tile = new TileIdentifier(tileX, tileY);
        }

        public TileIdentifier Tile { get; set; }

        public uint MapId { get; set; }

        public int TileX
        {
            get => Tile.X;
            set => Tile = new TileIdentifier(value, Tile.Y);
        }

        public int TileY
        {
            get => Tile.Y;
            set => Tile = new TileIdentifier(Tile.X, value);
        }
    }
}
