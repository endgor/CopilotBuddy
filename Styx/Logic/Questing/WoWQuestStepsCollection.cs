#nullable disable
using System.Runtime.InteropServices;
using Styx.WoWInternals;

namespace Styx.Logic.Questing
{
	/// <summary>
	/// Collection of quest steps (12 bytes).
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Size = 12)]
	public struct WoWQuestStepsCollection
	{
		private uint _padding;
		public uint StepsCount;
		private uint _stepsArrayPtr;

		public WoWQuestStep[] Steps
		{
			get
			{
				if (_stepsArrayPtr != 0U)
					return ObjectManager.Wow.ReadStructArray<WoWQuestStep>(_stepsArrayPtr, (int)StepsCount);
				return new WoWQuestStep[StepsCount];
			}
		}
	}
}
