using System;

namespace CopilotBuddy.Buddy.Overlay.Notifications
{
    public class ToastSettings
    {
        public bool DisplayTopToBottom { get; private set; }

        public ToastSettings(bool displayTopToBottom = false)
        {
            DisplayTopToBottom = displayTopToBottom;
        }
    }
}
