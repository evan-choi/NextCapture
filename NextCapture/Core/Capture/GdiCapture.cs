using NextCapture.Interop;
using NextCapture.Utils;
using System;
using System.Drawing;

namespace NextCapture.Core
{
    class GdiCapture : BaseCapture
    {
        protected override Bitmap OnCapture(Rectangle area)
        {
            return Capture(IntPtr.Zero, area);
        }

        protected override Bitmap OnCapture(IntPtr hwnd, Rectangle area)
        {
            return Capture(hwnd, area);
        }

        private Bitmap Capture(IntPtr hwnd, Rectangle area)
        {
            IntPtr hDest = IntPtr.Zero;

            IntPtr hdc = UnsafeNativeMethods.GetDC(hwnd);
            hDest = UnsafeNativeMethods.CreateCompatibleDC(hdc);

            IntPtr hBitmap = UnsafeNativeMethods.CreateCompatibleBitmap(hdc, area.Width, area.Height);

            IntPtr m_obit = UnsafeNativeMethods.SelectObject(hDest, hBitmap);

            UnsafeNativeMethods.BitBlt(hDest, 0, 0, area.Width, area.Height, hdc, area.X, area.Y, NativeMethods.TernaryRasterOperations.SRCCOPY);

            var bmp = Image.FromHbitmap(hBitmap);

            UnsafeNativeMethods.ReleaseDC(hwnd, hdc);
            UnsafeNativeMethods.DeleteDC(hDest);
            UnsafeNativeMethods.DeleteObject(hBitmap);

            // 메모리릭 임시 방편
            GC.Collect();

            return bmp;
        }
    }
}
