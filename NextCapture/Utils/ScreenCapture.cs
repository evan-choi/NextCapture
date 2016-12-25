using NextCapture.Interop;
using System;
using System.Drawing;

namespace NextCapture.Utils
{
    public static class ScreenCapture
    {
        public static Bitmap Capture(Rectangle area)
        {
            if (area.Width * area.Height <= 1)
                return null;

            IntPtr hDest = IntPtr.Zero;

            IntPtr hdc = UnsafeNativeMethods.GetDC(IntPtr.Zero);
            hDest = UnsafeNativeMethods.CreateCompatibleDC(hdc);

            IntPtr hBitmap = UnsafeNativeMethods.CreateCompatibleBitmap(hdc, area.Width, area.Height);

            IntPtr m_obit = UnsafeNativeMethods.SelectObject(hDest, hBitmap);

            UnsafeNativeMethods.BitBlt(hDest, 0, 0, area.Width, area.Height, hdc, area.X, area.Y, NativeMethods.TernaryRasterOperations.SRCCOPY);

            Bitmap img = Image.FromHbitmap(hBitmap);

            UnsafeNativeMethods.ReleaseDC(IntPtr.Zero, hdc);
            UnsafeNativeMethods.DeleteDC(hDest);
            UnsafeNativeMethods.DeleteObject(hBitmap);

            // 메모리릭 임시 방편
            GC.Collect();

            return img;
        }
    }
}
