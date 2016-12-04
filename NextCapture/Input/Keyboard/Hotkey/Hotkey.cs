using System;

namespace NextCapture.Input.Hotkey
{
    public class Hotkey
    {
        public string Name { get; internal set; }
        public IntPtr Target { get; set; } = IntPtr.Zero;

        public VKeys ModifierKey { get; set; }
        public VKeys[] SubKeys { get; set; }

        public bool Enabled { get; set; } = true;
        public HotKeyEvent Action { get; set; }

        internal bool IsGlobal
        {
            get
            {
                return Target == IntPtr.Zero;
            }
        }

        internal bool HasModifierKey
        {
            get
            {
                return ModifierKey != VKeys.None;
            }
        }

        public override string ToString()
        {
            return $"{Enabled}, {{{(ModifierKey == VKeys.None ? "" : $"{ModifierKey}, ") + string.Join(", ", SubKeys)}}}";
        }
    }
}
