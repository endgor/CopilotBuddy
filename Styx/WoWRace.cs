using System;

namespace Styx
{
	/// <summary>
	/// WoW Race IDs for WotLK 3.3.5a (build 12340)
	/// Note: Goblin (9) and Worgen (22) are Cataclysm races - not available in WotLK
	/// </summary>
	public enum WoWRace
	{
		Human = 1,
		Orc = 2,
		Dwarf = 3,
		NightElf = 4,
		Undead = 5,
		Tauren = 6,
		Gnome = 7,
		Troll = 8,
		Goblin, // Race ID 9 — not playable in WotLK but exists in ChrRaces.dbc (NPC race: Booty Bay, Ratchet, etc.)
		BloodElf,
		Draenei = 11,
		FelOrc = 12,
		Naga = 13,
		Broken = 14,
		Skeleton = 15
		// 22 = Worgen (Cataclysm only - DO NOT USE in WotLK)
	}
}
