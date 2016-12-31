using NextCapture.Model;
using NextCapture.Utils;
using System;
using System.Drawing;

namespace NextCapture.Core
{
    abstract class BaseCapture : ICaptureEngine
    {
        public event EventHandler<CaptureEventArgs> BeginCapture;
        public event EventHandler<CaptureEventArgs> EndCapture;

        public virtual bool CanCapture(IntPtr hwnd)
        {
            return hwnd != IntPtr.Zero;
        }

        public bool CanCapture(Rectangle area)
        {
            return area.Width * area.Height > 1;
        }

        public Bitmap Capture(Rectangle area)
        {
            return capture(IntPtr.Zero, area);
        }

        public Bitmap Capture(IntPtr hwnd)
        {
            var rect = WindowHelper.GetWindowRect(hwnd);
            rect.Location = Point.Empty;

            return capture(hwnd, rect);
        }

        private Bitmap capture(IntPtr hwnd, Rectangle area)
        {
            Bitmap result;

            var args = new CaptureEventArgs()
            {
                Target = hwnd,
                Bounds = area
            };

            BeginCapture?.Invoke(hwnd, args);

            try
            { 
                if (hwnd == IntPtr.Zero)
                {
                    result = OnCapture(area);
                }
                else
                {
                    result = OnCapture(hwnd, area);
                }
            }
            finally
            {
                EndCapture?.Invoke(hwnd, args);
            }

            return result;
        }

        protected abstract Bitmap OnCapture(Rectangle area);
        protected abstract Bitmap OnCapture(IntPtr hwnd, Rectangle area);
    }
}
