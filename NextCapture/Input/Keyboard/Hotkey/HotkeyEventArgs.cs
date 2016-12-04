using System;

namespace NextCapture.Input.Hotkey
{
    public class HotkeyEventArgs : EventArgs
    {
        public bool Handled { get; set; }

        public IntPtr Target { get; internal set; }

        public HotkeyEventArgs(IntPtr targetHwnd)
        {
            this.Target = targetHwnd;
        }
    }
}
