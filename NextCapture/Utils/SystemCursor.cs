using NextCapture.Interop;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading.Tasks;

namespace NextCapture.Utils
{
    internal static class SystemCursor
    {
        const string TempFileName = "blank_cursor";

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
            try
            {
                IntPtr result;

                if (!File.Exists(TempFileName))
                    File.WriteAllBytes(TempFileName, Properties.Resources.trans);

                result = UnsafeNativeMethods.LoadCursorFromFile(TempFileName);

                File.Delete(TempFileName);

                return result;
            }
            catch
            {
            }

            return IntPtr.Zero;
        }
    }
}