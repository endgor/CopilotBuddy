#nullable disable
using Tripper.XNAMath;

namespace Styx.Logic.Questing
{
	/// <summary>
	/// Integer 2D vector for quest area coordinates.
	/// NOTE: This struct exists in HB 3.3.5a for grid-based quest step coordinates.
	/// While not in HB 4.3.4, it's used by WoWQuestStep/QuestStepLocation for
	/// integer tile coordinates from quest cache data.
	/// </summary>
	public struct Vector2i
	{
		public int X;
		public int Y;

		public static implicit operator Vector2(Vector2i v)
		{
			return new Vector2((float)v.X, (float)v.Y);
		}
	}
}
