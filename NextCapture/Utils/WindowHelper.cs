using NextCapture.Interop;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NextCapture.Utils
{
    public static class WindowHelper
    {
        public static void ShowHighlight(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                return;

            IntPtr dc = UnsafeNativeMethods.GetWindowDC(hwnd);
            Rectangle rect = GetWindowRect(hwnd);

            UnsafeNativeMethods.SetROP2(dc, (int)NativeMethods.BinaryRasterOperations.R2_NOT);

            Color color = Color.FromArgb(0, 255, 0);
            int borderX = UnsafeNativeMethods.GetSystemMetrics((int)NativeMethods.SystemMetrics.SM_CXBORDER);
            IntPtr pen = UnsafeNativeMethods.CreatePen((int)NativeMethods.PenStyle.PS_INSIDEFRAME, 3 * borderX, (uint)color.ToArgb());

            IntPtr oldPen = UnsafeNativeMethods.SelectObject(dc, pen);
            IntPtr oldBrush = UnsafeNativeMethods.SelectObject(dc, UnsafeNativeMethods.GetStockObject((int)NativeMethods.StockObjects.NULL_BRUSH));

            UnsafeNativeMethods.Rectangle(dc, 0, 0, rect.Width, rect.Height);

            UnsafeNativeMethods.SelectObject(dc, oldBrush);
            UnsafeNativeMethods.SelectObject(dc, oldPen);

            UnsafeNativeMethods.ReleaseDC(hwnd, dc);
            UnsafeNativeMethods.DeleteObject(pen);
        }

        public static void HideHighlight(IntPtr hwnd)
        {
            ShowHighlight(hwnd);
            return;
            if (hwnd == IntPtr.Zero)
                return;

            UnsafeNativeMethods.InvalidateRect(hwnd, IntPtr.Zero, true);
            UnsafeNativeMethods.SendMessage(hwnd, NativeMethods.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool BringToTopWindow(IntPtr hwnd)
        {
            bool ret;

            ret = UnsafeNativeMethods.SetWindowPos(hwnd, -1, 0, 0, 0, 0, 0x0002 | 0x0001 | 0x0010);

            return ret;
        }

        public static int GetZOrder(IntPtr hwnd)
        {
            var lowestHwnd = UnsafeNativeMethods.GetWindow(hwnd, (int)NativeMethods.GetWindow.GW_HWNDLAST);

            var z = 0;
            var hwndTmp = lowestHwnd;

            while (hwndTmp != IntPtr.Zero)
            {
                if (hwnd == hwndTmp)
                {
                    return z;
                }

                hwndTmp = UnsafeNativeMethods.GetWindow(hwndTmp, (int)NativeMethods.GetWindow.GW_HWNDPREV);
                z++;
            }

            return -1;
        }

        public static bool BringToTopWindow(this Form window)
        {
            return BringToTopWindow(window.Handle);
        }

        public static int GetZOrder(this Form window)
        {
            return GetZOrder(window.Handle);
        }

        public static IntPtr ChildFromPoint(Point point)
        {
            IntPtr winHwnd = UnsafeNativeMethods.WindowFromPoint(point);
            return winHwnd;
            if (winHwnd == IntPtr.Zero)
                return IntPtr.Zero;

            if (!UnsafeNativeMethods.ScreenToClient(winHwnd, ref point))
                return IntPtr.Zero;

            IntPtr childHwnd = UnsafeNativeMethods.ChildWindowFromPointEx(winHwnd, point, 0);

            if (childHwnd == IntPtr.Zero)
                return winHwnd;

            if (!UnsafeNativeMethods.ClientToScreen(winHwnd, ref point))
                return IntPtr.Zero;

            int pScrWidth = UnsafeNativeMethods.GetSystemMetrics((int)NativeMethods.SystemMetrics.SM_CXFULLSCREEN);
            int pScrHeight = UnsafeNativeMethods.GetSystemMetrics((int)NativeMethods.SystemMetrics.SM_CYFULLSCREEN);

            int minSize = pScrWidth * pScrHeight;

            IntPtr result = childHwnd;

            while (childHwnd != IntPtr.Zero)
            {
                var rect = GetWindowRect(childHwnd);
                int childSize = rect.Width * rect.Height;

                if (rect.Contains(point) && minSize > childSize)
                {
                    minSize = childSize;
                    result = childHwnd;
                }

                childHwnd = UnsafeNativeMethods.GetWindow(childHwnd, (uint)NativeMethods.GetWindow.GW_HWNDNEXT);
            }
            
            return result;
        }

        public static Rectangle GetWindowRect(IntPtr hwnd)
        {
            var winRect = new NativeMethods.RECT();
            UnsafeNativeMethods.GetWindowRect(hwnd, ref winRect);

            return new Rectangle(winRect.Location, winRect.Size);
        }
    }
}
