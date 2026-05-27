using System;

namespace Styx.CommonBot.Routines
{
	[Flags]
	public enum CapabilityFlags : uint
	{
		None               = 0,
		Movement           = 1,
		MoveBehind         = 2,
		Facing             = 4,
		GapCloser          = 8,
		Aoe                = 16,
		Targeting          = 32,
		PetSummoning       = 64,
		PetUse             = 128,
		SpecialAttacks     = 256,
		Kiting             = 512,
		OffensiveDispel    = 1024,
		DefensiveDispel    = 2048,
		MultiMobPull       = 4096,
		Taunting           = 8192,
		Interrupting       = 16384,
		OffensiveCooldowns = 32768,
		DefensiveCooldowns = 65536,
		All                = 131071,
	}
}
