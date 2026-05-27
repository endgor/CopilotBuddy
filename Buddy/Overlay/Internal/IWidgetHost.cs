using System;
using CopilotBuddy.Buddy.Overlay.Controls;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal interface IWidgetHost
    {
        bool AddWidget(OverlayControl control);
        void RemoveWidget(OverlayControl control);
    }
}
