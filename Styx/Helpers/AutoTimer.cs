using System;
using System.Diagnostics;

namespace Styx.Helpers
{
    /// <summary>
    /// HB 6.2.3: Styx.Common.Helpers.AutoTimer
    /// Logs elapsed time of a using() block via WriteDiagnostic.
    /// Usage: using (new AutoTimer("MyOperation")) { ... }
    /// Only logs if elapsed > 5ms.
    /// </summary>
    public class AutoTimer : IDisposable
    {
        public Stopwatch Watch { get; private set; }

        private readonly string _codePart;

        public AutoTimer(string codePart)
        {
            Watch = Stopwatch.StartNew();
            _codePart = codePart;
        }

        public AutoTimer(string codePartFormat, params object[] args)
            : this(string.Format(codePartFormat, args))
        {
        }

        public void Dispose()
        {
            Watch.Stop();
            string text;
            if (Watch.ElapsedMilliseconds > 60000L)
            {
                text = Watch.Elapsed.TotalMinutes.ToString("N3") + " minutes";
            }
            else if (Watch.ElapsedMilliseconds > 1000L)
            {
                text = Watch.Elapsed.TotalSeconds.ToString("N3") + " seconds";
            }
            else
            {
                if (Watch.ElapsedMilliseconds <= 5L)
                    return;
                text = Watch.ElapsedMilliseconds + " ms";
            }
            Logging.WriteDiagnostic("{0} took {1}", _codePart, text);
        }
    }
}
