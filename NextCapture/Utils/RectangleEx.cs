using System;
using System.Drawing;

namespace NextCapture.Utils
{
    public static class RectangleEx 
    {
        public static Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle()
            {
                X = Math.Min(p1.X, p2.X),
                Y = Math.Min(p1.Y, p2.Y),
                Width = Math.Abs(p1.X - p2.X),
                Height = Math.Abs(p1.Y - p2.Y)
            };
        }
    }
}
