using NextCapture.Database;
using System;
using System.Windows.Forms;

namespace NextCapture
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Utils.CursorUtil.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Utils.CursorUtil.Reset();
        }
    }
}