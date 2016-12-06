using System;
using NextCapture.Interop;
using System.Windows.Forms;

namespace NextCapture.Utils
{
    public static class HwndHelper
    {
        public static bool BringToTopWindow(this Form window)
        {
            bool ret;

            ret = UnsafeNativeMethods.SetWindowPos(window.Handle, -1, 0, 0, 0, 0, 0x0002 | 0x0001 | 0x0010);

            return ret;
        }

        public static int GetZOrder(this Form window)
        {
            const uint GW_HWNDPREV = 3;
            const uint GW_HWNDLAST = 1;

            var lowestHwnd = UnsafeNativeMethods.GetWindow(window.Handle, GW_HWNDLAST);

            var z = 0;
            var hwndTmp = lowestHwnd;
            while (hwndTmp != IntPtr.Zero)
            {
                if (window.Handle == hwndTmp)
                {
                    return z;
                }

                hwndTmp = UnsafeNativeMethods.GetWindow(hwndTmp, GW_HWNDPREV);
                z++;
            }

            return -1;
        }
    }
}
