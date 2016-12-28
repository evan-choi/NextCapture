using NextCapture.Interop;
using System;
using System.Drawing;

namespace NextCapture.Utils
{
    public class CaptureEventArgs : EventArgs
    {
        public IntPtr Target { get; set; }
        public Rectangle Bounds { get; set; }
        public Bitmap Bitmap { get; set; }
        public bool Handled { get; set; }
    }
    
    public static class ScreenCapture
    {
        public static event EventHandler<CaptureEventArgs> BeginCapture;
        public static event EventHandler<CaptureEventArgs> EndCapture;

        public static Bitmap Capture(IntPtr hwnd)
        {
            var rect = WindowHelper.GetWindowRect(hwnd);
            rect.Location = Point.Empty;

            return Capture(hwnd, rect);
        }

        public static Bitmap Capture(Rectangle area)
        {
            return Capture(IntPtr.Zero, area);
        }

        private static Bitmap Capture(IntPtr hwnd, Rectangle area)
        {
            if (area.Width * area.Height <= 1)
                return null;

            var args = new CaptureEventArgs()
            {
                Target = hwnd,
                Bounds = area
            };

            BeginCapture?.Invoke(hwnd, args);

            try
            {
                if (!args.Handled)
                {
                    IntPtr hDest = IntPtr.Zero;

                    IntPtr hdc = UnsafeNativeMethods.GetDC(hwnd);
                    hDest = UnsafeNativeMethods.CreateCompatibleDC(hdc);

                    IntPtr hBitmap = UnsafeNativeMethods.CreateCompatibleBitmap(hdc, area.Width, area.Height);

                    IntPtr m_obit = UnsafeNativeMethods.SelectObject(hDest, hBitmap);

                    UnsafeNativeMethods.BitBlt(hDest, 0, 0, area.Width, area.Height, hdc, area.X, area.Y, NativeMethods.TernaryRasterOperations.SRCCOPY);

                    args.Bitmap = Image.FromHbitmap(hBitmap);

                    UnsafeNativeMethods.ReleaseDC(hwnd, hdc);
                    UnsafeNativeMethods.DeleteDC(hDest);
                    UnsafeNativeMethods.DeleteObject(hBitmap);

                    // 메모리릭 임시 방편
                    GC.Collect();
                }
            }
            finally
            {
                EndCapture?.Invoke(hwnd, args);
            }

            return args.Bitmap;
        }
    }
}
