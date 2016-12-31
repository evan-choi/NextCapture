using NextCapture.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextCapture.Windows
{
    class TestWindow : LayeredWindow
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();


        public TestWindow() : base()
        {
        }

        public void Focus(IntPtr hwnd)
        {
            var rect = WindowHelper.GetWindowRect(hwnd);
            var cRect = WindowHelper.GetClientRect(hwnd);

            this.Location = rect.Location + 
                new Size(
                    (int)((rect.Width - cRect.Width) / 2f), 0);
            this.Size = cRect.Size;

            if (Width * Height == 0)
                return;

            using (var bmp = new Bitmap(Width, Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Red);
                    DrawBitmap(bmp, 100);
                }
            }

            this.Activate();
        }
    }
}
