using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CopilotBuddy.Buddy.Overlay.Internal
{
    internal static class NativeMethods
    {
        public const int WS_EX_TRANSPARENT = 32;
        public const int WS_EX_TOOLWINDOW = 128;
        public const int WS_EX_NOACTIVATE = 134217728;
        public const int GWL_EXSTYLE_OFFSET = -20;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y,
            int width, int height, SetWindowPosFlags flags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO info);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowRelationship cmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(WindowsHookType hookType,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int FormatMessage(FormatMessageFlags dwFlags, IntPtr lpSource,
            uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr arguments);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState,
            byte[] lpChar, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(out bool pfEnabled);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, out Point lpPoint);

        public static uint GetWindowThreadId(IntPtr hWnd)
        {
            uint processId;
            return GetWindowThreadProcessId(hWnd, out processId);
        }

        public static uint GetWindowProcessId(IntPtr hWnd)
        {
            uint result;
            GetWindowThreadProcessId(hWnd, out result);
            return result;
        }

        public static WINDOWINFO GetWindowInfo(IntPtr hWnd)
        {
            WINDOWINFO result = new WINDOWINFO(true);
            GetWindowInfo(hWnd, ref result);
            return result;
        }

        public static string GetWin32ErrorMessage(int errorCode)
        {
            try
            {
                IntPtr buffer = IntPtr.Zero;
                if (FormatMessage(
                    FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER |
                    FormatMessageFlags.FORMAT_MESSAGE_IGNORE_INSERTS |
                    FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM,
                    IntPtr.Zero, (uint)errorCode, 0, ref buffer, 0, IntPtr.Zero) == 0)
                {
                    return "Unable to get error code string from System - Error " +
                        Marshal.GetLastWin32Error().ToString();
                }
                string text = Marshal.PtrToStringAnsi(buffer);
                buffer = LocalFree(buffer);
                return text;
            }
            catch (Exception ex)
            {
                return "Unable to get error code string from System -> " + ex.ToString();
            }
        }
    }
}
