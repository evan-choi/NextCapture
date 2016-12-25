using System;
using System.Runtime.InteropServices;

using NextCapture.Interop;
using System.Collections.Generic;

using MouseStruct = NextCapture.Interop.NativeMethods.MOUSEHOOKSTRUCT;

namespace NextCapture.Input
{
    internal class MouseHook : IHook<MouseStruct>, IDisposable
    {
        public bool IsHooked { get; private set; }

        public IList<IHookFilter<MouseStruct>> Filters { get; } = new List<IHookFilter<MouseStruct>>();

        private IntPtr mHookHandle;
        private NativeMethods.HookProc mHookProc;
        
        ~MouseHook()
        {
            this.Dispose();
        }

        public bool Hook()
        {
            if (mHookHandle != IntPtr.Zero)
                return false;
                
            IntPtr module = SafeNativeMethods.LoadLibrary("user32");

            mHookProc = new NativeMethods.HookProc(_Hook);

            mHookHandle = UnsafeNativeMethods.SetWindowsHookEx(
                (int)UnsafeNativeMethods.HookType.WH_MOUSE_LL,
                mHookProc,
                module,
                0);

            IsHooked = true;

            return IsHooked;
        }

        public bool UnHook()
        {
            if (IsHooked)
            {
                UnsafeNativeMethods.UnhookWindowsHookEx(mHookHandle);
                
                mHookHandle = IntPtr.Zero;

                IsHooked = false;
            }

            return !IsHooked;
        }

        private IntPtr _Hook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == 0)
            {
                var mhs = (MouseStruct)Marshal.PtrToStructure(lParam, typeof(MouseStruct));
                
                foreach (var filter in Filters)
                {
                    if (filter.HookProc(wParam, lParam, mhs))
                    {
                        return new IntPtr(1);
                    }
                }
            }

            return UnsafeNativeMethods.CallNextHookEx(mHookHandle, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            UnHook();
        }
    }
}
