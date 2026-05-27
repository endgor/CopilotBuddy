using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal abstract class OverlayWindowBase : TransparentWindow
    {
        private readonly int _extendedWindowStyle;
        private bool _isPulsing;

        public event EventHandler Pulse;

        public IntPtr Handle { get; private set; }
        public Process AttachedProcess { get; private set; }

        protected Rectangle ScreenBounds { get; private set; }
        protected ScaleTransform ScaleTransform { get; private set; }
        protected Canvas Canvas { get; private set; }
        protected Grid LayoutGrid { get; private set; }

        public double ScaleX => ScaleTransform.ScaleX;
        public double ScaleY => ScaleTransform.ScaleY;

        protected OverlayWindowBase(Process attachedProcess, int windowStyle = 0)
        {
            AttachedProcess = attachedProcess;
            _extendedWindowStyle = windowStyle;
            ScaleTransform = new ScaleTransform(1.0, 1.0);
            ScreenBounds = Screen.PrimaryScreen.Bounds;

            Canvas = new Canvas
            {
                Background = System.Windows.Media.Brushes.Transparent,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            };

            LayoutGrid = new Grid
            {
                Background = System.Windows.Media.Brushes.Transparent,
                LayoutTransform = ScaleTransform,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            };
            LayoutGrid.Children.Add(Canvas);
            Content = LayoutGrid;

            CompositionTarget.Rendering += OnCompositionTargetRendering;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Handle = new WindowInteropHelper(this).Handle;
            if (_extendedWindowStyle != 0)
            {
                int style = NativeMethods.GetWindowLong(Handle, NativeMethods.GWL_EXSTYLE_OFFSET);
                style = (style | _extendedWindowStyle | NativeMethods.WS_EX_TOOLWINDOW);
                NativeMethods.SetWindowLong(Handle, NativeMethods.GWL_EXSTYLE_OFFSET, style);
            }
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            if (_isPulsing)
                return;
            try
            {
                _isPulsing = true;
                OnPulseOverride();
                EventHandler handler = Pulse;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
            finally
            {
                _isPulsing = false;
            }
        }

        protected internal void SetPosition(int x, int y, int width, int height, IntPtr insertAfter)
        {
            using (Dispatcher.DisableProcessing())
            {
                NativeMethods.SetWindowPos(Handle, insertAfter, x, y, width, height,
                    SetWindowPosFlags.DoNotActivate);
                ScaleTransform.ScaleX = (double)((float)width / (float)ScreenBounds.Width);
                ScaleTransform.ScaleY = (double)((float)height / (float)ScreenBounds.Height);
            }
        }

        protected virtual void OnPulseOverride()
        {
        }
    }
}
