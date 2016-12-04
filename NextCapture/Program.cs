using NextCapture.Input.Mouse;
using System;
using System.Windows.Forms;

namespace NextCapture
{
    static class Program
    {
        static MouseHook mHook;

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

            Utils.CursorUtil.Init();

            mHook = new MouseHook();
            mHook.Hook();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            mHook.UnHook();

            Utils.CursorUtil.Reset();
        }
    }
}