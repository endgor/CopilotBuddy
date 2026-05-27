using Styx.Combat.CombatRoutine;
using Styx.WoWInternals;

namespace Styx.CommonBot.Routines
{
	public sealed class InvalidRoutineWrapper : CombatRoutine
	{
		public override string Name => "Invalid Routine";

		public override WoWClass Class => StyxWoW.Me.Class;

		public override void Initialize()
		{
		}
	}
}
