using System;

namespace Styx.Logic.BehaviorTree
{
	/// <summary>
	/// Event args for TreeRoot.OnGoalTextChanged — same pattern as StatusTextChangedEventArgs.
	/// </summary>
	public class GoalTextChangedEventArgs : EventArgs
	{
		internal GoalTextChangedEventArgs(string oldGoal, string newGoal)
		{
			OldGoal = oldGoal;
			NewGoal = newGoal;
		}

		public string OldGoal { get; set; }
		public string NewGoal { get; set; }
	}
}
