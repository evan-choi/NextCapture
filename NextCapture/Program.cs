using NextCapture.Core;
using NextCapture.Input;
using NextCapture.Input.Hotkey;
using NextCapture.Utils;
using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace NextCapture
{
    static class Program
    {
        public static MouseHook MouseHook;
        public static OSXCapture OSXCapture;
        public static NotifyIcon Notify;

        static SoundPlayer ShutterPlayer;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Initialize();
            InitializeNotify();

            Application.Run(new MainWindow());
        }

        public static void Close()
        {
            if (MouseHook != null)
            {
                MouseHook.UnHook();
                Notify.Visible = false;

                SystemCursor.Show();

                MouseHook = null;
                Notify = null;
            }

            Application.Exit();
        }

        public static void ShutterSound()
        {
            ShutterPlayer.Stop();
            ShutterPlayer.Play();
        }

        private static void InitializeNotify()
        {
            var ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem($"{Config.AppName} 정보", NotifyIcon_Info));
            ctx.MenuItems.Add(new MenuItem("-"));
            ctx.MenuItems.Add(new MenuItem("종료", NotifyIcon_Close));

            Notify = new NotifyIcon()
            {
                Icon = Properties.Resources.icon,
                Text = $"{Config.AppName} v.{Config.Version.ToString()}",
                ContextMenu = ctx
            };

            Notify.Visible = true;
            Notify.ShowBalloonTip(2000, Config.AppName, "Running!", ToolTipIcon.Info);
        }

        private static void Initialize()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            ScreenCapture.BeginCapture += ScreenCapture_BeginCapture;

            Dispatcher.Init();
            HotkeyManager.Init();
            SystemCursor.Show();

            ShutterPlayer = new SoundPlayer(Properties.Resources.shutter);

            OSXCapture = new OSXCapture();

            MouseHook = new MouseHook();
            MouseHook.Hook();

            MouseHook.Filters.Add(OSXCapture);

#if DEBUG
            HotkeyManager.Register("Force_Close", new Hotkey()
            {
                ModifierKey = VKeys.Alt,
                SubKeys = new[] { VKeys.Escape },
                Action = new HotKeyEvent((s, e) =>
                {
                    Program.Close();
                    (e as HotkeyEventArgs).Handled = true;
                })
            });
#endif
        }

        private static void ScreenCapture_BeginCapture(object sender, CaptureEventArgs e)
        {
            ShutterSound();
        }

        private static void NotifyIcon_Info(object sender, EventArgs e)
        {
            var infoWindow = new InfoWindow();

            infoWindow.Show();
            infoWindow.Activate();
        }

        private static void NotifyIcon_Close(object sender, EventArgs e)
        {
            Program.Close();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Program.Close();
        }
    }
}