using System;

namespace Tripper.Navigation
{
    // HB 6.2.3 Tripper/Navigation/MapLoadedEventArgs.cs
    public class MapLoadedEventArgs : EventArgs
    {
        public MapLoadedEventArgs()
        {
        }

        public MapLoadedEventArgs(uint mapId)
        {
            Names = mapId == 0 ? Array.Empty<string>() : new[] { mapId.ToString() };
            IsTiled = true;
        }

        public string[] Names { get; set; } = Array.Empty<string>();

        public bool IsTiled { get; set; }
    }
}
