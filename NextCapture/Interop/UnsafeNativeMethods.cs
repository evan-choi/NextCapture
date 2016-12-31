using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace NextCapture.Interop
{
    internal static class UnsafeNativeMethods
    {
        [DllImport(ExternDll.User32)]
        public static extern bool GetCursorPos(out Point point);

        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern bool SystemParametersInfo(NativeMethods.SPI uiAction, uint uiParam, IntPtr pvParam, NativeMethods.SPIF fWinIni);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr SetWindowsHookEx(int hookid, NativeMethods.HookProc pfnhook, IntPtr hinst, int threadid);

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhook);

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern IntPtr GetFocus();

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(ExternDll.User32)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst,
            ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc,
            Int32 crKey, ref NativeMethods.BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport(ExternDll.Gdi32)]
        public static extern uint Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr GetStockObject(int fnObject);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);

        [DllImport(ExternDll.Gdi32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport(ExternDll.Gdi32, EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport(ExternDll.Gdi32, EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, NativeMethods.TernaryRasterOperations dwRop);

        [DllImport(ExternDll.Gdi32)]
        public static extern int SetROP2(IntPtr hdc, int fnDrawMode);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport(ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr LoadCursorFromFile(string lpFileName);
        
        [DllImport(ExternDll.User32)]
        public static extern int DestroyCursor(IntPtr hCursor);

        [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CopyIcon(IntPtr hcur);

        [DllImport(ExternDll.User32)]
        public static extern bool SetSystemCursor(IntPtr hCursor, uint id);
        
        [DllImport(ExternDll.DwmAPI)]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref NativeMethods.MARGINS pMarInset);

        [DllImport(ExternDll.DwmAPI)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport(ExternDll.DwmAPI)]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr WindowFromPoint(Point point);

        [DllImport(ExternDll.User32)]
        public static extern bool ScreenToClient(IntPtr handle, ref Point point);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr ChildWindowFromPointEx(IntPtr hWndParent, Point pt, uint uFlags);

        [DllImport(ExternDll.User32)]
        public static extern bool ClientToScreen(IntPtr hwnd, ref Point lpPoint);

        [DllImport(ExternDll.User32)]
        public static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport(ExternDll.User32)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref NativeMethods.RECT lpRect);

        [DllImport(ExternDll.User32)]
        public static extern bool GetClientRect(IntPtr hWnd, ref NativeMethods.RECT lpRect);

        [DllImport(ExternDll.User32)]
        public static extern int GetSystemMetrics(int smIndex);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(ExternDll.User32)]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport(ExternDll.User32)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport(ExternDll.User32)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref NativeMethods.MONITORINFO lpmi);

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14,
        }

        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            KF_EXTENDED = 0x0100,
            KF_ALTDOWN = 0x2000,
            LLKHF_DOWN = 0x00,
            LLKHF_EXTENDED = KF_EXTENDED >> 8,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = KF_ALTDOWN >> 8,
            LLKHF_UP = 0x80,
        }
    }
}
