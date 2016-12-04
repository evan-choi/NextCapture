using Microsoft.Win32;
using NextCapture.Interop;
using System.Collections.Generic;
using System.Linq;

namespace NextCapture.Utils
{
    internal enum CursorTypes
    {
        AppStarting,
        Arrow,
        Crosshair,
        Hand,
        Help,
        IBeam,
        No,
        NWPen,
        SizeAll,
        SizeNESW,
        SizeNS,
        SizeNWSE,
        SizeWE,
        UpArrow,
        Wait,
    }

    internal static class CursorUtil
    {
        internal static Dictionary<CursorTypes, string> cursorBackup;
        internal static RegistryKey cursorReg;

        static CursorUtil()
        {
            cursorBackup = new Dictionary<CursorTypes, string>();
            cursorReg = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors", true);

            Backup();
        }

        public static bool ChangeArrowCursor(CursorTypes cursor, string fileName)
        {
            if (HasCursor(cursor.ToString()))
            {
                cursorReg.SetValue(cursor.ToString(), fileName);
                Update();

                return true;
            }

            return false;
        }

        public static void Update()
        {
            bool value = false;

            UnsafeNativeMethods.SystemParametersInfo(
                NativeMethods.SPI.SETCURSORS,
                0,
                ref value,
                NativeMethods.SPIF.UPDATEINIFILE | NativeMethods.SPIF.SENDCHANGE);
        }

        public static void Backup()
        {
            cursorBackup.Clear();

            foreach (string v in cursorReg
                .GetValueNames()
                .Where(v => cursorReg.GetValueKind(v) != RegistryValueKind.DWord))
            {
                var e = v.ToEnum<CursorTypes>();

                if (e.HasValue)
                {
                    cursorBackup[e.Value] = (string)cursorReg.GetValue(v);

                    // TODO: Save to local database
                }
            }
        }

        public static void Reset()
        {
            foreach (var kv in cursorBackup)
            {
                cursorReg.SetValue(kv.Key.ToString(), kv.Value);
            }
        }

        private static bool HasCursor(string cursorName)
        {
            return cursorReg
                .GetValueNames()
                .Count(n => n.AnyEquals(cursorName)) > 0;
        }
    }
}