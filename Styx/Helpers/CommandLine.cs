using System;

namespace Styx.Helpers
{
    /// <summary>
    /// Provides access to command-line arguments passed to the process.
    /// Ported from HB 4.3.4 Styx.Helpers.CommandLine.
    /// HbUser/HbPass removed — no auth in CopilotBuddy.
    /// </summary>
    public static class CommandLine
    {
        static CommandLine()
        {
            Arguments = new Arguments(Arguments.Tokenize(Environment.CommandLine));
        }

        /// <summary>Gets the parsed command-line arguments.</summary>
        public static Arguments Arguments { get; private set; }

        /// <summary>Path to a profile to load at startup (-loadprofile).</summary>
        public static string AutoLoadProfile => Arguments.GetString("loadprofile");

        /// <summary>Name of the bot to start automatically (-botname).</summary>
        public static string AutoStartBotName => Arguments.GetString("botname");

        /// <summary>Process ID to attach to automatically (-pid).</summary>
        public static int AutoAttachProcessId => Arguments.GetInt("pid");

        /// <summary>Name of the custom class to select automatically (-customclass).</summary>
        public static string AutoSelectCustomClass => Arguments.GetString("customclass");
    }
}
