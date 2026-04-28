using Styx.Helpers;

namespace Bots.DungeonBuddy.Helpers
{
    public static class Logger
    {
        public static void Write(string message) => Logging.Write(message);
        public static void Write(string format, params object[] args) => Logging.Write(format, args);
        public static void WriteDiagnostic(string message) => Logging.WriteDiagnostic(message);
        public static void WriteDiagnostic(string format, params object[] args) => Logging.WriteDiagnostic(format, args);

        public static void WriteError(Error error)
        {
            if (error == null)
                return;

            switch (error.Type)
            {
                case ErrorType.Error:
                    Logging.WriteError("[DungeonBuddy-Error]: " + error);
                    break;
                case ErrorType.Warning:
                    Logging.Write("[DungeonBuddy-Warning]: {0}", error);
                    break;
            }
        }
    }
}
