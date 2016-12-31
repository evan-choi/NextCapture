using NextCapture.Core;
using NextCapture.Input;
using NextCapture.Input.Hotkey;
using NextCapture.Model;
using NextCapture.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NextCapture
{
    static class Program
    {
        public static MouseHook MouseHook;
        public static OSXCapture OSXCapture;
        public static NotifyIcon Notify;

        static Dictionary<string, Assembly> assms = new Dictionary<string, Assembly>();
        static SoundPlayer ShutterPlayer;

        [STAThread]
        static void Main()
        {
            Initialize();
            InitializeNotify();

            var tw = new Windows.TestWindow();
            tw.Show();

            OSXCapture.FocusedWindowChanged += (s, e) =>
            {
                tw.Focus(e.NewValue);
            };

            Application.Run(new MainWindow());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ForceClose();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ForceClose();
        }

        private static void ForceClose()
        {
            MouseHook.UnHook();
            Keyboard.Hook.UnHook();
            SystemCursor.Show();

            Application.ExitThread();
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
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Dispatcher.Init();
            HotkeyManager.Init();
            SystemCursor.Show();

            LoadAssembly("SharpDX.dll");
            LoadAssembly("SharpDX.Direct3D11.dll");
            LoadAssembly("SharpDX.DXGI.dll");

            ShutterPlayer = new SoundPlayer(Properties.Resources.shutter);

            OSXCapture = new OSXCapture();
            
            foreach (var engine in OSXCapture.CaptureEngines)
                engine.BeginCapture += ScreenCapture_BeginCapture;

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

        private static void LoadAssembly(string name)
        {
            var current = Assembly.GetExecutingAssembly();

            string resource = current.GetManifestResourceNames()
                .First(n => Regex.IsMatch(n, name, RegexOptions.IgnoreCase));

            var stream = current.GetManifestResourceStream(resource);
            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, buffer.Length);

            var loadedAssm = Assembly.Load(buffer);

            assms[loadedAssm.FullName] = loadedAssm;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (assms.ContainsKey(args.Name))
                return assms[args.Name];

            return null;
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