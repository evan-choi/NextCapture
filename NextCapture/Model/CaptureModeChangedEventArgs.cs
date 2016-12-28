using NextCapture.Core;

namespace NextCapture
{
    class CaptureModeChangedEventArgs : ValueChangedEventArgs<CaptureMode>
    {
        public CaptureModeChangedEventArgs()
        {
        }

        public CaptureModeChangedEventArgs(CaptureMode old, CaptureMode @new) : base(old, @new)
        {
        }
    }
}
