using NextCapture.Input;
using NextCapture.Input.Hotkey;
using NextCapture.Utils;
using System;
using System.IO;
using System.Windows.Forms;

namespace NextCapture
{
    static class Program
    {
        public static MouseHook mHook;

        [STAThread]
        static void Main()
        {
            Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void Initialize()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            HotkeyManager.Init();
            CursorUtil.Init();
            
            mHook = new MouseHook();
            mHook.Hook();

            // TODO: Debug code to Logic
            File.WriteAllBytes("temp.ani", Properties.Resources.trans);

            foreach(CursorTypes c in Enum.GetValues(typeof(CursorTypes)))
            {
                CursorUtil.ChangeCursor(c, "temp.ani");
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            mHook.UnHook();

            CursorUtil.Reset();
        }
    }
}