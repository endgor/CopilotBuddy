using System;
using System.Diagnostics;
using System.Windows;

namespace CopilotBuddy.Buddy.Overlay.Notifications
{
    public abstract class ToastUIComponent : OverlayUIComponentBase
    {
        internal Stopwatch Stopwatch { get; private set; }
        public TimeSpan DisplayDuration { get; protected set; }

        protected ToastUIComponent() : base(false)
        {
            Stopwatch = new Stopwatch();
        }
    }
}
