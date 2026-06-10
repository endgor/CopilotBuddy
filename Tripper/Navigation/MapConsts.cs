namespace Tripper.Navigation
{
    /// <summary>
    /// Navigation map constants.
    /// WoW uses a 64x64 ADT grid with each ADT = 533.3333 yards.
    /// CopilotBuddy uses 1x1 MaNGOS-style tiles: one Detour tile per ADT.
    /// </summary>
    public static class MapConsts
    {
        public const float TileSize = 533.3333f;
    }
}
