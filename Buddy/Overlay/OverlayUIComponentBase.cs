using System;
using System.Windows;

namespace CopilotBuddy.Buddy.Overlay
{
    public abstract class OverlayUIComponentBase
    {
        public bool IsHitTestable { get; private set; }
        public abstract FrameworkElement GuiElement { get; }

        protected OverlayUIComponentBase(bool isHitTestable)
        {
            IsHitTestable = isHitTestable;
        }

        protected internal virtual void Update()
        {
        }
    }
}
