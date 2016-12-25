using NextCapture.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NextCapture
{
    public class DragWindow : LayeredWindow
    {
        Point startPosition;

        public Point DragStartPosition { get { return startPosition; } }

        public DragWindow() : base()
        {
        }

        public void Setup(Point startPosition)
        {
            this.startPosition = startPosition;
        }

        public void UpdateLayout(Point position)
        {
            var rect = RectangleEx.GetRectangle(startPosition, position);

            this.Location = rect.Location;
            this.Size = rect.Size;

            if (rect.Width * rect.Height == 0)
                return;

            var bmp = new Bitmap(rect.Width, rect.Height);

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb((byte)(255 * 0.15), Color.Black));
                g.DrawRectangle(Pens.White, 0, 0, rect.Width - 1, rect.Height - 1);
            }

            DrawBitmap(bmp, 255);
            bmp.Dispose();
        }
    }
}