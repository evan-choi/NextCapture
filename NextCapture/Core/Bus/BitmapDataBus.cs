using System;
using System.Drawing;

namespace NextCapture.Core
{
    internal abstract class BitmapDataBus : IDataBus<Bitmap>
    {
        public bool Enabled { get; set; } = true;

        public bool SendData(Bitmap data)
        {
            if (!Enabled)
                return false;

            return OnSendData(data);
        }

        public virtual bool OnSendData(Bitmap data)
        {
            return true;
        }
    }
}