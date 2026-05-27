using System;

namespace Styx.Logic.BehaviorTree
{
    /// <summary>
    /// HB 6.2.3: Event args for TreeRoot.OnShutdownRequested.
    /// Fired before the application exits so plugins can clean up.
    /// </summary>
    public class ShutdownRequestedEventArgs : EventArgs
    {
        public ShutdownRequestedEventArgs(HonorbuddyExitCode exitCode, bool closeWow)
        {
            ExitCode = exitCode;
            CloseWow = closeWow;
        }

        public HonorbuddyExitCode ExitCode { get; }
        public bool CloseWow { get; }
    }
}
