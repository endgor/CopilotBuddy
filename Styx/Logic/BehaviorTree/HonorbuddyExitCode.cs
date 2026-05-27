namespace Styx.Logic.BehaviorTree
{
    /// <summary>
    /// Exit code passed to TreeRoot.Shutdown() — compatible with HB plugin API.
    /// </summary>
    public enum HonorbuddyExitCode
    {
        Default = 0,
        InactivityDetector = 1,
        UserLogout = 2,
        UserExit = 3,
    }
}
