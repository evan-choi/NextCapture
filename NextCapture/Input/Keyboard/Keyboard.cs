using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCapture.Input
{
    public static class Keyboard
    {
        static VKeys[] BaseModifierKeys = new VKeys[]
        {
            VKeys.LShiftKey, VKeys.RShiftKey,
            VKeys.LControlKey, VKeys.RControlKey,
            VKeys.LMenu, VKeys.RMenu
        };

        static internal KeyboardHook Hook;

        static Keyboard()
        {
            Init();
        }

        public static void Init()
        {
            if (Hook == null)
            {
                Hook = new KeyboardHook();
                Hook.Hook();
            }
        }

        public static VKeys ModifierKeys
        {
            get
            {
                return Hook.ModifierKeys;
            }
        }

        public static bool IsShift
        {
            get
            {
                return ModifierKeys.HasFlag(VKeys.Shift);
            }
        }

        public static bool IsControl
        {
            get
            {
                return ModifierKeys.HasFlag(VKeys.Control);
            }
        }

        public static bool IsAlt
        {
            get
            {
                return ModifierKeys.HasFlag(VKeys.Alt);
            }
        }

        public static bool IsModifierKey(VKeys key)
        {
            return BaseModifierKeys.Contains(key);
        }

        public static bool IsPressed(VKeys key)
        {
            return Hook.GetDownedKeys().Contains(key);
        }

        public static bool IsReleased(VKeys key)
        {
            return !IsPressed(key);
        }
    }
}
