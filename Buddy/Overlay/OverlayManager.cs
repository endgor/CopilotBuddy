using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using CopilotBuddy.Buddy.Overlay.Controls;
using CopilotBuddy.Buddy.Overlay.Internal;
using CopilotBuddy.Buddy.Overlay.Notifications;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;

namespace CopilotBuddy.Buddy.Overlay
{
    public class OverlayManager
    {
        private const int MaxToastDurationMs = 25000;
        private const int MaxVisibleToasts = 12;

        private readonly List<OverlayUIComponent> _widgets = new List<OverlayUIComponent>();
        private readonly List<ToastUIComponent> _activeToasts = new List<ToastUIComponent>();
        private readonly Action<ToastUIComponent> _addToastAction;
        private readonly Action<OverlayUIComponent> _addWidgetAction;
        private readonly Action<OverlayUIComponent> _removeWidgetAction;
        private readonly List<OverlayUIComponentBase> _pendingComponents = new List<OverlayUIComponentBase>();

        private WidgetOverlayWindow _widgetWindow;
        private ToastOverlayWindow _toastWindow;
        private RECT _lastClientRect;
        private uint _overlayThreadId;
        private Thread _overlayThread;
        private LowLevelKeyboardProc _keyboardHookProc;
        private IntPtr _keyboardHookHandle;
        private byte[] _keyboardState;
        private OverlayControl _lastClickedControl;
        private bool _isActive;
        private bool _isActivating;
        private readonly Dispatcher _customDispatcher;

        public ToastSettings ToastSettings { get; private set; }
        public Process AttachedProcess { get; private set; }

        public Dispatcher Dispatcher
        {
            get
            {
                Dispatcher result;
                if ((result = _customDispatcher) == null)
                {
                    if (_overlayThread == null)
                        return null;
                    result = Dispatcher.FromThread(_overlayThread);
                }
                return result;
            }
        }

        public bool IsActive => _customDispatcher != null || _overlayThread != null;

        public bool IsDesktopCompositionEnabled
        {
            get
            {
                bool enabled;
                return NativeMethods.DwmIsCompositionEnabled(out enabled) == 0 && enabled;
            }
        }

        public double UnscaledOverlayWidth => (double)Screen.PrimaryScreen.Bounds.Width;
        public double UnscaledOverlayHeight => (double)Screen.PrimaryScreen.Bounds.Height;

        public OverlayManager(Process attachedProcess, ToastSettings toastSettings = null,
            Dispatcher customDispatcher = null)
        {
            ToastSettings = (toastSettings ?? new ToastSettings(false));
            AttachedProcess = attachedProcess;
            _customDispatcher = customDispatcher;
            _addToastAction = new Action<ToastUIComponent>(OnAddToast);
            _addWidgetAction = new Action<OverlayUIComponent>(OnAddWidget);
            _removeWidgetAction = new Action<OverlayUIComponent>(OnRemoveWidget);
        }

        private void OnControlPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (NativeMethods.GetForegroundWindow() != AttachedProcess.MainWindowHandle)
            {
                NativeMethods.SetForegroundWindow(AttachedProcess.MainWindowHandle);
                UpdateOverlayPosition();
            }

            OverlayControl control = (OverlayControl)sender;
            if (object.Equals(_lastClickedControl, control))
                return;

            _lastClickedControl = control;
            int count = _widgets.Count;
            System.Windows.Controls.Panel.SetZIndex(control, count);

            foreach (OverlayUIComponent component in _widgets)
            {
                OverlayControl ctrl = component.Control;
                int zIndex = System.Windows.Controls.Panel.GetZIndex(ctrl);
                if (zIndex > 0 && !ctrl.Equals(control))
                    System.Windows.Controls.Panel.SetZIndex(ctrl, zIndex - 1);
            }
        }

        private IntPtr LowLevelKeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            System.Windows.Controls.Primitives.TextBoxBase textBox = Keyboard.FocusedElement as System.Windows.Controls.Primitives.TextBoxBase;
            if (nCode >= 0 &&
                NativeMethods.GetForegroundWindow() == AttachedProcess.MainWindowHandle &&
                _lastClickedControl != null &&
                textBox != null)
            {
                uint vkCode = (uint)Marshal.ReadInt32(lParam);
                uint scanCode = NativeMethods.MapVirtualKey(vkCode, 0U);
                WindowMessage message = (WindowMessage)((int)wParam);
                IntPtr lParamForKey = (IntPtr)((int)(((message == WindowMessage.KEY_DOWN) ? 1U : 3221225473U) | scanCode << 16));

                MSG msg = new MSG
                {
                    hwnd = _widgetWindow.Handle,
                    lParam = lParamForKey,
                    message = (int)wParam,
                    wParam = (IntPtr)((long)((ulong)vkCode))
                };

                bool handled = ComponentDispatcher.RaiseThreadMessage(ref msg);

                if (_lastClickedControl != null && message == WindowMessage.KEY_DOWN && vkCode == 27U)
                {
                    _lastClickedControl = null;
                    Keyboard.ClearFocus();
                    handled = true;
                }

                if (!handled && message == WindowMessage.KEY_DOWN)
                {
                    NativeMethods.GetKeyboardState(_keyboardState);
                    byte[] chars = new byte[2];
                    if (NativeMethods.ToAscii(vkCode, scanCode, _keyboardState, chars, 0U) == 1)
                    {
                        msg.message = 258;
                        msg.wParam = (IntPtr)((int)chars[0]);
                        handled = ComponentDispatcher.RaiseThreadMessage(ref msg);
                    }
                }

                if (handled)
                    return (IntPtr)1;
            }
            return NativeMethods.CallNextHookEx(_keyboardHookHandle, nCode, wParam, lParam);
        }

        private void OnWidgetWindowClosing(object sender, CancelEventArgs e)
        {
            UninstallKeyboardHook();
        }

        private void OnPulse(object sender, EventArgs e)
        {
            if (AttachedProcess.HasExited)
                return;

            for (int i = _activeToasts.Count - 1; i >= 0; i--)
            {
                ToastUIComponent toast = _activeToasts[i];
                if (toast.Stopwatch.Elapsed > toast.DisplayDuration)
                {
                    _activeToasts.RemoveAt(i);
                    _toastWindow.RemoveToastElement(toast.GuiElement);
                }
                else
                {
                    toast.Update();
                }
            }

            foreach (OverlayUIComponent component in _widgets)
            {
                component.Update();
            }

            UpdateOverlayPosition();
        }

        private void OnPreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OldFocus is System.Windows.Controls.Primitives.TextBoxBase && !(e.NewFocus is System.Windows.Controls.Primitives.TextBoxBase))
                UninstallKeyboardHook();
        }

        private void OnPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus is System.Windows.Controls.Primitives.TextBoxBase)
                InstallKeyboardHook();
        }

        private unsafe IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 71) // WM_WINDOWPOSCHANGED
            {
                WINDOWPOS* pos = (WINDOWPOS*)((void*)lParam);
                _widgetWindow.SetPosition(pos->x, pos->y, pos->cx, pos->cy, pos->hwndInsertAfter);
            }
            return IntPtr.Zero;
        }

        private void InstallKeyboardHook()
        {
            if (_keyboardHookHandle != IntPtr.Zero)
                return;
            _keyboardHookHandle = NativeMethods.SetWindowsHookEx(
                WindowsHookType.WH_KEYBOARD_LL, _keyboardHookProc, IntPtr.Zero, 0U);
            _keyboardState = new byte[256];
        }

        private void UninstallKeyboardHook()
        {
            if (_keyboardHookHandle == IntPtr.Zero)
                return;
            NativeMethods.UnhookWindowsHookEx(_keyboardHookHandle);
            _keyboardHookHandle = IntPtr.Zero;
            _keyboardState = null;
        }

        public void AddUIComponent(OverlayUIComponent uiElement)
        {
            lock (this)
            {
                if (!IsActive)
                    return;
                if (!_isActive)
                {
                    _pendingComponents.Add(uiElement);
                }
                else
                {
                    Dispatcher.BeginInvoke(_addWidgetAction, new object[] { uiElement });
                }
            }
        }

        public void RemoveUIComponent(OverlayUIComponent uiElement)
        {
            lock (this)
            {
                if (!IsActive)
                    return;
                if (!_isActive)
                {
                    _pendingComponents.Remove(uiElement);
                }
                else
                {
                    Dispatcher.BeginInvoke(_removeWidgetAction, new object[] { uiElement });
                }
            }
        }

        public void AddToast(string text, int durationMs = 7000)
        {
            string capturedText = text;
            AddToast(new Func<string>(() => capturedText),
                TimeSpan.FromMilliseconds((double)durationMs),
                Colors.Gold, Colors.Lime,
                new System.Windows.Media.FontFamily("Times New Roman"));
        }

        public void AddToast(Func<string> textProducer, TimeSpan duration, Color color,
            Color shadowColor, FontFamily fontFamily)
        {
            AddToast(new TextToastComponent(textProducer, duration, color, shadowColor,
                fontFamily, FontWeights.UltraBold, 40.0));
        }

        public void AddToast(Func<string> textProducer, TimeSpan duration, Color color,
            Color shadowColor, FontFamily fontFamily, FontWeight fontWeight, double fontSize)
        {
            AddToast(new TextToastComponent(textProducer, duration, color, shadowColor,
                fontFamily, fontWeight, fontSize));
        }

        public void AddToast(ToastUIComponent toast)
        {
            if (toast.DisplayDuration.TotalMilliseconds <= 0.0 ||
                toast.DisplayDuration.TotalMilliseconds > MaxToastDurationMs)
            {
                throw new ArgumentException(string.Format(
                    "duration needs to be bigger than 0 and smaller or equal to {0}", MaxToastDurationMs));
            }

            lock (this)
            {
                if (!IsActive)
                    return;
                if (!_isActive)
                {
                    _pendingComponents.Add(toast);
                    return;
                }
                Dispatcher.BeginInvoke(_addToastAction, new object[] { toast });
            }
        }

        public void Activate()
        {
            lock (this)
            {
                if (!_isActive && !_isActivating)
                {
                    _isActivating = true;
                    if (_customDispatcher != null)
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            new Action(InitializeOverlay));
                    }
                    else
                    {
                        _overlayThread = new Thread(new ThreadStart(RunOverlayThread))
                        {
                            IsBackground = true
                        };
                        _overlayThread.SetApartmentState(ApartmentState.STA);
                        _overlayThread.Start();
                    }
                }
            }
        }

        public void Deactivate()
        {
            lock (this)
            {
                if (IsActive)
                {
                    if (_isActive && _customDispatcher == null)
                    {
                        Dispatcher.BeginInvokeShutdown(DispatcherPriority.Render);
                    }
                    _overlayThread = null;
                    _isActive = false;
                }
            }
        }

        private void InitializeOverlay()
        {
            lock (this)
            {
                try
                {
                    _overlayThreadId = NativeMethods.GetCurrentThreadId();
                    _widgetWindow = new WidgetOverlayWindow(AttachedProcess);
                    _toastWindow = new ToastOverlayWindow(AttachedProcess, ToastSettings);

                    _widgetWindow.Closing += OnWidgetWindowClosing;
                    _widgetWindow.PreviewGotKeyboardFocus += OnPreviewGotKeyboardFocus;
                    _widgetWindow.PreviewLostKeyboardFocus += OnPreviewLostKeyboardFocus;

                    _widgetWindow.Show();
                    _toastWindow.Show();

                    (PresentationSource.FromVisual(_toastWindow) as HwndSource)
                        .AddHook(new HwndSourceHook(HwndSourceHook));

                    _keyboardHookProc = new LowLevelKeyboardProc(LowLevelKeyboardCallback);
                    _widgetWindow.Pulse += OnPulse;

                    foreach (OverlayUIComponentBase component in _pendingComponents)
                    {
                        ToastUIComponent toast = component as ToastUIComponent;
                        if (toast != null)
                            _addToastAction(toast);
                        else
                            _addWidgetAction((OverlayUIComponent)component);
                    }
                    _pendingComponents.Clear();
                    _isActive = true;
                }
                finally
                {
                    _isActivating = false;
                }
            }
        }

        private void RunOverlayThread()
        {
            InitializeOverlay();
            Dispatcher.Run();
        }

        private void UpdateOverlayPosition()
        {
            IntPtr windowPrev = NativeMethods.GetWindow(AttachedProcess.MainWindowHandle,
                GetWindowRelationship.GwHwndprev);
            RECT clientRect;
            NativeMethods.GetClientRect(AttachedProcess.MainWindowHandle, out clientRect);
            System.Drawing.Point screenPoint;
            NativeMethods.ClientToScreen(AttachedProcess.MainWindowHandle, out screenPoint);
            clientRect.Left += screenPoint.X;
            clientRect.Right += screenPoint.X;
            clientRect.Top += screenPoint.Y;
            clientRect.Bottom += screenPoint.Y;

            if (NativeMethods.GetWindowThreadId(windowPrev) == _overlayThreadId && clientRect == _lastClientRect)
                return;

            _lastClientRect = clientRect;
            int x = clientRect.Left;
            int y = clientRect.Top;
            int width = clientRect.Right - clientRect.Left;
            int height = clientRect.Bottom - clientRect.Top;
            _widgetWindow.SetPosition(x, y, width, height, windowPrev);
            _toastWindow.SetPosition(x, y, width, height, windowPrev);
        }

        private void OnAddToast(ToastUIComponent toast)
        {
            if (_activeToasts.Count >= MaxVisibleToasts)
            {
                _activeToasts.RemoveAt(0);
                _toastWindow.RemoveToastElementAt(0);
            }
            if (!_toastWindow.AddToastElement(toast.GuiElement))
                return;
            toast.Stopwatch.Start();
            _activeToasts.Add(toast);
        }

        private void OnAddWidget(OverlayUIComponent component)
        {
            IWidgetHost host;
            if (!component.IsHitTestable)
                host = _toastWindow;
            else
                host = _widgetWindow;

            if (!host.AddWidget(component.Control))
                return;
            component.Control.PreviewMouseDown += OnControlPreviewMouseDown;
            _widgets.Add(component);
        }

        private void OnRemoveWidget(OverlayUIComponent component)
        {
            IWidgetHost host;
            if (!component.IsHitTestable)
                host = _toastWindow;
            else
                host = _widgetWindow;

            host.RemoveWidget(component.Control);
            component.Control.PreviewMouseDown -= OnControlPreviewMouseDown;
            _widgets.Remove(component);
            if (component.Control.Equals(_lastClickedControl))
                _lastClickedControl = null;
        }
    }
}
