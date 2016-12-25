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
                data.Save($"{Directory}\\스크린샷 {DateTime.Now.ToString("yyyy-MM-dd tt h.mm.ss.fff")}.png");
            }
            catch
            {
            }

            return true;
        }
    }
}