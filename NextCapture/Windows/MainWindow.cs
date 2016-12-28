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
using System.Media;

namespace NextCapture
{
    public sealed class MainWindow : LayeredWindow, IHookFilter<MouseStruct>
    {
        int lastZOrder = 0;
        
        SolidBrush whiteBrush;
        DragWindow dWindow;

        public MainWindow() : base()
        {
            InitializeDrag();
            
            whiteBrush = new SolidBrush(Color.FromArgb((int)(255 * 0.4), Color.White));

            Program.MouseHook.Filters.Add(this);
            Program.OSXCapture.CaptureModeChanged += OSXCapture_CaptureModeChanged;
            Program.OSXCapture.BeginDragCapture += OSXCapture_BeginDragCapture;
            Program.OSXCapture.EndDragCapture += OSXCapture_EndDragCapture;
            Program.OSXCapture.FocusedWindowChanged += OSXCapture_FocusedWindowChanged;
        }
        
        private void InitializeDrag()
        {
            dWindow = new DragWindow();
            this.Owner = dWindow;
        }

        void HideDragLayer()
        {
            dWindow.Hide();
        }

        void ShowDragLayer(Point pos)
        {
            dWindow.Setup(pos);
            dWindow.UpdateLayout(pos);
            dWindow.Show();
        }

        private void OSXCapture_EndDragCapture(object sender, Point e)
        {
            ClearLayer();
            HideDragLayer();
        }
        
        private void OSXCapture_BeginDragCapture(object sender, Point e)
        {
            ShowDragLayer(e);
            UpdateLayout(e, Point.Empty);
        }

        private void OSXCapture_FocusedWindowChanged(object sender, FocusedWindowEventArgs e)
        {
            WindowHelper.HideHighlight(e.OldValue);
            WindowHelper.ShowHighlight(e.NewValue);
        }

        private void OSXCapture_CaptureModeChanged(object sender, CaptureModeChangedEventArgs e)
        {
            switch (e.NewValue)
            {
                case Core.CaptureMode.Unknown:
                    if (e.OldValue == Core.CaptureMode.Window)
                    {
                        var mw = new MessageWindow(Config.AppName, "Comming soon!");

                        mw.Show();
                        mw.Activate();
                    }

                    SystemCursor.Show();
                    HideDragLayer();
                    ClearLayer();
                    break;

                case Core.CaptureMode.Drag:
                case Core.CaptureMode.Window:
                    SystemCursor.Hide();
                    UpdateLayout(MousePosition, MousePosition);
                    break;

                case Core.CaptureMode.FullScreen:
                    break;
            }
        }
        
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.BringToTopWindow();
        }

        private void UpdateLayout(Point position, Point displayPosition)
        {
            this.SuspendLayout();

            Bitmap bmp;
            Point offset = Point.Empty;

            if (Program.OSXCapture.CaptureMode == Core.CaptureMode.Window)
            {
                bmp = new Bitmap(Properties.Resources.camera);
            }
            else
            {
                DrawCrossHair(out bmp, position, displayPosition);
                offset = new Point(15, 8);
            }

            this.Location = position - 
                new Size((int)Math.Floor(bmp.Width / 2f), (int)Math.Floor(bmp.Height / 2f)) + (Size)offset;

            DrawBitmap(bmp, 255);
            bmp.Dispose();

            // ISSUE: Catch the behind to window
            UpdateOnTop();

            this.ResumeLayout(false);
        }

        void DrawCrossHair(out Bitmap bitmap, Point position, Point displayPosition)
        {
            var cross = Properties.Resources.Cross;
            bitmap = new Bitmap(cross.Width + 30, cross.Height + 16);

            using (var g = Graphics.FromImage(bitmap))
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
                Rectangle screen = Screen.FromPoint(MousePosition).Bounds;

                Point position = ClipPoint(new Point(data.x, data.y), screen);
                Point display = position;
                
                switch (Program.OSXCapture.CaptureMode)
                {
                    case Core.CaptureMode.Drag:
                        // Drag Layer Update
                        if (Program.OSXCapture.IsDragging)
                        {
                            var rect = RectangleEx.GetRectangle(dWindow.DragStartPosition, MousePosition);

                            display = (Point)rect.Size;
                            
                            dWindow?.UpdateLayout(position);
                        }
                        break;

                    //case Core.CaptureMode.Window:
                    //    IntPtr hwnd = WindowHelper.ChildFromPoint(MousePosition);

                    //    if (hwnd != Program)
                    //    {
                    //        WindowHelper.HideHighlight(focusedWindow);
                    //        WindowHelper.ShowHighlight(hwnd);

                    //        focusedWindow = hwnd;
                    //    }
                        
                    //    break;
                }

                this.UpdateLayout(position, display);
            }

            return false;
        }

        Point ClipPoint(Point point, Rectangle rect)
        {
            point.X = Math.Min(Math.Max(point.X, rect.Left), rect.Right);
            point.Y = Math.Min(Math.Max(point.Y, rect.Top), rect.Bottom);

            return point;
        }
    }
}
