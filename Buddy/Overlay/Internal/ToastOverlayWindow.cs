using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CopilotBuddy.Buddy.Overlay.Controls;
using CopilotBuddy.Buddy.Overlay.Notifications;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal sealed class ToastOverlayWindow : OverlayWindowBase, IToastHost, IWidgetHost
    {
        private readonly StackPanel _toastStack;

        public ToastOverlayWindow(Process attachedProcess, ToastSettings toastSettings)
            : base(attachedProcess, NativeMethods.WS_EX_TRANSPARENT)
        {
            Grid grid = new Grid
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Background = System.Windows.Media.Brushes.Transparent
            };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.4, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.6, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });

            _toastStack = new StackPanel
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };
            Grid.SetColumn(_toastStack, 1);
            grid.Children.Add(_toastStack);
            LayoutGrid.Children.Add(grid);

            ApplyToastSettings(toastSettings);
        }

        private void ApplyToastSettings(ToastSettings settings)
        {
            System.Windows.VerticalAlignment alignment = settings.DisplayTopToBottom
                ? System.Windows.VerticalAlignment.Top
                : System.Windows.VerticalAlignment.Bottom;
            _toastStack.VerticalAlignment = alignment;
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

        public bool AddToastElement(FrameworkElement element)
        {
            if (_toastStack.Children.Contains(element))
                return false;
            _toastStack.Children.Add(element);
            return true;
        }

        public void RemoveToastElement(FrameworkElement element)
        {
            _toastStack.Children.Remove(element);
        }

        public void RemoveToastElementAt(int index)
        {
            _toastStack.Children.RemoveAt(index);
        }
    }
}
