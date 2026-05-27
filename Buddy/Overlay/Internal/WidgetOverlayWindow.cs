using System;
using System.Diagnostics;
using CopilotBuddy.Buddy.Overlay.Controls;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal sealed class WidgetOverlayWindow : OverlayWindowBase, IWidgetHost
    {
        public WidgetOverlayWindow(Process attachedProcess)
            : base(attachedProcess, NativeMethods.WS_EX_NOACTIVATE)
        {
        }

        public bool AddWidget(OverlayControl control)
        {
            if (Canvas.Children.Contains(control))
                return false;
            Canvas.Children.Add(control);
            return true;
        }

        public void RemoveWidget(OverlayControl control)
        {
            Canvas.Children.Remove(control);
        }
    }
}
