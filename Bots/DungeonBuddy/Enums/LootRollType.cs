namespace Bots.DungeonBuddy.Enums
{
    /// <summary>
    /// Controls how the bot rolls on group loot (START_LOOT_ROLL event).
    /// Values match the WoW API's RollOnLoot(rollId, type) argument:
    ///   1 = Need, 2 = Greed, 3 = Pass.
    /// </summary>
    public enum LootRollType
    {
        Need  = 1,
        Greed = 2,
        Pass  = 3
    }
}
