using System;
using System.Windows;
using CopilotBuddy.Buddy.Overlay.Controls;

namespace CopilotBuddy.Buddy.Overlay
{
    public abstract class OverlayUIComponent : OverlayUIComponentBase
    {
        public sealed override FrameworkElement GuiElement => Control;
        public abstract OverlayControl Control { get; }

        protected OverlayUIComponent(bool isHitTestable) : base(isHitTestable)
        {
        }
    }
}
