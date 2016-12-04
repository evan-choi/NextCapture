using Microsoft.Win32;
using NextCapture.Database;
using NextCapture.Interop;
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
        internal static RegistryKey cursorReg;

        static CursorUtil()
        {
            Init();
        }

        public static void Init()
        {
            if (cursorReg == null)
            {
                cursorReg = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors", true);

                Reset();
                Backup();
            }
        }

        public static bool ChangeCursor(CursorTypes cursor, string fileName)
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
            if (DB.Cursors.Count > 0)
                return;

            foreach (string v in cursorReg
                .GetValueNames()
                .Where(v => cursorReg.GetValueKind(v) != RegistryValueKind.DWord))
            {
                var e = v.ToEnum<CursorTypes>();

                if (e.HasValue)
                {
                    string value = (string)cursorReg.GetValue(v);
                    
                    DB.Cursors[v] = value;
                }
            }
        }

        public static void Reset()
        {
            foreach (var kv in DB.Cursors)
                cursorReg.SetValue(kv.Key.ToString(), kv.Value);

            DB.Cursors.Clear();

            Update();
        }

        private static bool HasCursor(string cursorName)
        {
            return cursorReg
                .GetValueNames()
                .Count(n => n.AnyEquals(cursorName)) > 0;
        }
    }
}