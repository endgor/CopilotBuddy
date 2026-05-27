using System;
using System.Windows;
using System.Windows.Media;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal abstract class TransparentWindow : Window
    {
        protected TransparentWindow()
        {
            ShowInTaskbar = false;
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            Background = System.Windows.Media.Brushes.Transparent;
            Style = null;
        }
    }
}
