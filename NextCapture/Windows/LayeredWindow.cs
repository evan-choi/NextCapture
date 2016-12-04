using NextCapture.Interop;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using WSEX = NextCapture.Interop.NativeMethods.WS_EX;
using WS = NextCapture.Interop.NativeMethods.WS;
using GWL = NextCapture.Interop.NativeMethods.GWL;

namespace NextCapture
{
    public class LayeredWindow : Form
    {
        const byte AC_SRC_OVER = 0x00;
        const byte AC_SRC_ALPHA = 0x01;
        const int ULW_ALPHA = 0x02;

        public LayeredWindow()
        {
            this.SuspendLayout();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            var ws = (WS)UnsafeNativeMethods.GetWindowLong(Handle, (int)GWL.STYLE);
            var wsex = (WSEX)UnsafeNativeMethods.GetWindowLong(Handle, (int)GWL.EXSTYLE);
            
            wsex |= WSEX.NOACTIVATE | WSEX.LAYERED | WSEX.TRANSPARENT;

            UnsafeNativeMethods.SetWindowLong(Handle, (int)GWL.STYLE, (int)ws);
            UnsafeNativeMethods.SetWindowLong(Handle, (int)GWL.EXSTYLE, (int)wsex);
        }

        public bool IsSupportLayered()
        {
            return OSFeature.Feature.IsPresent(OSFeature.LayeredWindows);
        }

        protected void DrawBitmap(Bitmap bitmap, int opacity)
        {
            IntPtr screenDc = UnsafeNativeMethods.GetDC(IntPtr.Zero);
            IntPtr memDc = UnsafeNativeMethods.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                hOldBitmap = UnsafeNativeMethods.SelectObject(memDc, hBitmap);
                
                var newSize = new Size(bitmap.Width, bitmap.Height);
                var sourceLocation = new Point(0, 0);
                var newLocation = new Point(this.Left, this.Top);

                var blend = new NativeMethods.BLENDFUNCTION()
                {
                    BlendOp = AC_SRC_OVER,
                    BlendFlags = 0,
                    SourceConstantAlpha = (byte)opacity,
                    AlphaFormat = AC_SRC_ALPHA
                };
                
                UnsafeNativeMethods.UpdateLayeredWindow(
                    this.Handle,    
                    screenDc,       
                    ref newLocation,
                    ref newSize,    
                    memDc,                       
                    ref sourceLocation, 
                    0,               
                    ref blend,       
                    ULW_ALPHA);
            }
            finally
            {
                UnsafeNativeMethods.ReleaseDC(IntPtr.Zero, screenDc);

                if (hBitmap != IntPtr.Zero)
                {
                    UnsafeNativeMethods.SelectObject(memDc, hOldBitmap);
                    UnsafeNativeMethods.DeleteObject(hBitmap);
                }

                UnsafeNativeMethods.DeleteDC(memDc);
            }
        }
    }
}