using NextCapture.Interop;
using System;

namespace NextCapture.Input.Mouse
{
    static class Mouse
    {
        public static void LeftDown(int x, int y)
        {
            UnsafeNativeMethods.mouse_event((int)NativeMethods.MouseEventFlags.LEFTDOWN, (uint)x, (uint)y, 0, UIntPtr.Zero);
        }

        public static void LeftUp(int x, int y)
        {
            UnsafeNativeMethods.mouse_event((int)NativeMethods.MouseEventFlags.LEFTUP, (uint)x, (uint)y, 0, UIntPtr.Zero);
        }
    }
}
