using System;

namespace NextCapture
{
    class FocusedWindowEventArgs : ValueChangedEventArgs<IntPtr>
    {
        public FocusedWindowEventArgs()
        {
        }

        public FocusedWindowEventArgs(IntPtr old, IntPtr @new) : base(old, @new)
        {
        }
    }
}
