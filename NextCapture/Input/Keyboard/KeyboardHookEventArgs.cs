using System;

namespace NextCapture.Input
{
    internal class KeyboardHookEventArgs : EventArgs
    {
        public VKeys KeyCode { get; }
        public VKeys ModifierKeys { get; internal set; }
        public bool IsFirst { get; internal set; }

        public bool Control { get; set; }
        public bool Shift { get; set; }
        public bool Alt { get; set; }

        public bool Handled { get; set; }

        public KeyboardHookEventArgs(VKeys keyData)
        {
            this.KeyCode = keyData;

            Shift = (keyData == VKeys.ShiftKey || keyData == VKeys.LShiftKey || keyData == VKeys.RShiftKey);
            Control = (keyData == VKeys.ControlKey || keyData == VKeys.LControlKey || keyData == VKeys.RControlKey);
            Alt = (keyData == VKeys.Menu || keyData == VKeys.LMenu || keyData == VKeys.RMenu);

            ModifierKeys = VKeys.None;

            if (Shift)
                ModifierKeys = VKeys.Shift;

            if (Control)
                ModifierKeys = VKeys.Control;

            if (Alt)
                ModifierKeys = VKeys.Alt;
        }
    }
}
