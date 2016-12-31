using NextCapture.Model;
using System;
using System.Drawing;

namespace NextCapture.Core
{
    interface ICaptureEngine
    {
        event EventHandler<CaptureEventArgs> BeginCapture;
        event EventHandler<CaptureEventArgs> EndCapture;

        bool CanCapture(IntPtr hwnd);

        bool CanCapture(Rectangle area);

        Bitmap Capture(IntPtr hwnd);

        Bitmap Capture(Rectangle area);
    }
}
