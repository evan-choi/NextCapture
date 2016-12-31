using System;
using System.Drawing;

namespace NextCapture.Model
{
    public class CaptureEventArgs : EventArgs
    {
        public IntPtr Target { get; set; }
        public Rectangle Bounds { get; set; }
        public Bitmap Bitmap { get; set; }
        public bool Handled { get; set; }
    }
}
