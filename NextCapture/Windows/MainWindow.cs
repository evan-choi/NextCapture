using NextCapture.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MouseStruct = NextCapture.Interop.NativeMethods.MOUSEHOOKSTRUCT;
using NextCapture.Interop;
using System.Runtime.InteropServices;
using NextCapture.Utils;
using System.IO;

namespace NextCapture
{
    public sealed class MainWindow : LayeredWindow, IHookFilter<MouseStruct>
    {
        int lastZOrder = 0;

        NotifyIcon notify;
        SolidBrush whiteBrush;

        DragWindow dWindow;

        public MainWindow() : base()
        {
            InitializeNotify();
            InitializeDrag();

            whiteBrush = new SolidBrush(Color.FromArgb((int)(255 * 0.4), Color.White));

            Program.MouseHook.Filters.Add(this);
            Program.OSXCapture.CaptureModeChanged += OSXCapture_CaptureModeChanged;
            Program.OSXCapture.BeginDragCapture += OSXCapture_BeginDragCapture;
            Program.OSXCapture.EndDragCapture += OSXCapture_EndDragCapture;
        }

        private void InitializeDrag()
        {
            dWindow = new DragWindow();
            this.Owner = dWindow;
        }

        private void InitializeNotify()
        {
            var ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("NextCapture 정보", NotifyIcon_Info));
            ctx.MenuItems.Add(new MenuItem("-"));
            ctx.MenuItems.Add(new MenuItem("종료", NotifyIcon_Close));

            notify = new NotifyIcon()
            {
                Icon = Properties.Resources.icon,
                Text = "NextCapture",
                ContextMenu = ctx
            };

            notify.Visible = true;
        }

        void DestoryDragLayer()
        {
            dWindow.Hide();
        }

        void CreateDragLayer(Point pos)
        {
            dWindow.Setup(pos);
            dWindow.UpdateLayout(pos);
            dWindow.Show();
        }

        private void OSXCapture_EndDragCapture(object sender, Point e)
        {
            DestoryDragLayer();
        }

        private void OSXCapture_BeginDragCapture(object sender, Point e)
        {
            CreateDragLayer(e);
            UpdateLayout(e, Point.Empty);
        }

        private void OSXCapture_CaptureModeChanged(object sender, EventArgs e)
        {
            switch (Program.OSXCapture.CaptureMode)
            {
                case Core.CaptureMode.Unknown:
                    SystemCursor.Show();
                    DestoryDragLayer();

                    using (var bmp = new Bitmap(1, 1))
                        DrawBitmap(bmp, 0);

                    break;

                case Core.CaptureMode.Drag:
                    SystemCursor.Hide();

                    UpdateLayout(MousePosition, MousePosition);

                    break;
            }
        }

        private void NotifyIcon_Info(object sender, EventArgs e)
        {
            var infoWindow = new InfoWindow();

            infoWindow.Show();
            infoWindow.Activate();
        }

        private void NotifyIcon_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            notify.Visible = false;
            base.OnClosed(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.BringToTopWindow();
        }

        private void UpdateLayout(Point position, Point displayPosition)
        {
            var cross = Properties.Resources.Cross;
            var bmp = new Bitmap(cross.Width + 30, cross.Height + 15);

            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(cross, new Rectangle(0, 0, cross.Width, cross.Height));

                using (var f = new Font(Font.FontFamily, 8))
                {
                    var xPt = new Point(cross.Width - 6, cross.Height - 6);
                    var yPt = new Point(cross.Width - 6, cross.Height + 4);

                    g.DrawString(displayPosition.X.ToString(), f, whiteBrush, xPt + new Size(1, 1));
                    g.DrawString(displayPosition.Y.ToString(), f, whiteBrush, yPt + new Size(1, 1));

                    g.DrawString(displayPosition.X.ToString(), f, Brushes.Black, xPt);
                    g.DrawString(displayPosition.Y.ToString(), f, Brushes.Black, yPt);
                }   
            }

            this.Location = position - new Size(cross.Width / 2, cross.Height / 2);

            DrawBitmap(bmp, 255);
            bmp.Dispose();

            // ISSUE: Catch the behind to window
            UpdateOnTop();
        }

        void UpdateOnTop()
        {
            int zorder = this.GetZOrder();

            if (zorder < lastZOrder)
            {
                this.BringToTopWindow();   
            }

            lastZOrder = zorder;
        }

        bool IHookFilter<MouseStruct>.HookProc(IntPtr wParam, IntPtr lParam, MouseStruct data)
        {
            if (Program.OSXCapture.CaptureMode != Core.CaptureMode.Unknown)
            {
                Point position = MousePosition;
                Point display = position;

                if (Program.OSXCapture.IsDragging)
                {
                    var rect = RectangleEx.GetRectangle(dWindow.DragStartPosition, MousePosition);

                    display = (Point)rect.Size;
                }

                this.UpdateLayout(position, display);
            }
            
            if (dWindow != null && Program.OSXCapture.IsDragging)
            {
                dWindow.UpdateLayout(MousePosition);
            }

            return false;
        }
    }
}
