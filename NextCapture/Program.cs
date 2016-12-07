using NextCapture.Core;
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
        public static MouseHook MouseHook;
        public static OSXCapture OSXCapture;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Initialize();

            Application.Run(new Form1());
        }

        private static void Initialize()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Dispatcher.Init();
            HotkeyManager.Init();
            CursorUtil.Init();

            OSXCapture = new OSXCapture();

            MouseHook = new MouseHook();
            MouseHook.Hook();

            MouseHook.Filters.Add(OSXCapture);
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            MouseHook.UnHook();

            CursorUtil.Reset();
        }
    }
}