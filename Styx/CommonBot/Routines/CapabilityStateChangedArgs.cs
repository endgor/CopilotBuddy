using System;

namespace Styx.CommonBot.Routines
{
	public sealed class CapabilityStateChangedArgs : EventArgs
	{
		public CapabilityStateChangedArgs(CapabilityFlags capability, CapabilityState oldState, CapabilityState newState)
		{
			Capability = capability;
			OldState   = oldState;
			NewState   = newState;
		}

		public CapabilityFlags Capability { get; }
		public CapabilityState OldState   { get; }
		public CapabilityState NewState   { get; }
	}
}
