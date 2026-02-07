using System;

namespace Styx.Logic.BehaviorTree
{
	/// <summary>
	/// Event args for TreeRoot.OnStatusTextChanged — same as HB 4.3.4
	/// </summary>
	public class StatusTextChangedEventArgs : EventArgs
	{
		internal StatusTextChangedEventArgs(string oldStatus, string newStatus)
		{
			this.OldStatus = oldStatus;
			this.NewStatus = newStatus;
		}

		public string OldStatus { get; set; }
		public string NewStatus { get; set; }
	}
}
