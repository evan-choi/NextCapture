using NextCapture.Interop;
using System;
using System.IO;
using System.Windows.Forms;

namespace NextCapture.Utils
{
    internal static class SystemCursor
    {
        public static void Show()
        {
            UnsafeNativeMethods.SystemParametersInfo(
                NativeMethods.SPI.SETCURSORS, 0, IntPtr.Zero, NativeMethods.SPIF.None);
        }

        public static void Hide()
        {
            IntPtr hCursor = LoadBlankCursor();

            foreach (NativeMethods.IDC idc in Enum.GetValues(typeof(NativeMethods.IDC)))
            {
                IntPtr hCursorCopy = UnsafeNativeMethods.CopyIcon(hCursor);

                UnsafeNativeMethods.SetSystemCursor(hCursorCopy, (uint)idc);
                UnsafeNativeMethods.DestroyCursor(hCursorCopy);
            }
        }

        private static IntPtr LoadBlankCursor()
        {
            using (var ms = new MemoryStream(Properties.Resources.trans))
            {
                using (var cursor = new Cursor(ms))
                {
                    return cursor.CopyHandle();
                }
            }
        }
    }
}