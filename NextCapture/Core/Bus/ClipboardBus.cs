using System.Drawing;
using System.Windows.Forms;

namespace NextCapture.Core
{
    class ClipboardBus : BitmapDataBus
    {
        public override bool OnSendData(Bitmap data)
        {
            try
            {
                Clipboard.SetImage(data);
            }
            catch
            {
            }

            return true;
        }
    }
}
