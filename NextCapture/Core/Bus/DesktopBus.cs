using System;
using System.Drawing;

namespace NextCapture.Core
{
    class DesktopBus : BitmapDataBus
    {
        private string Directory;

        public DesktopBus()
        {
            this.Directory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        public override bool OnSendData(Bitmap data)
        {
            try
            {
                data.Save($"{Directory}\\hello.png");
            }
            catch
            {
            }

            return true;
        }
    }
}