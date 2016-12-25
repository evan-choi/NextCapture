using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using NextCapture.Interop;
using KeyboardStruct = NextCapture.Interop.NativeMethods.KBDLLHOOKSTRUCT;

namespace NextCapture.Input
{
    internal class KeyboardHook : IHook<KeyboardStruct>, IDisposable
    {
        public event EventHandler<KeyboardHookEventArgs> KeyDown;
        public event EventHandler<KeyboardHookEventArgs> KeyUp;

        public bool IsHooked { get; private set; }

        public IList<IHookFilter<KeyboardStruct>> Filters { get; } = new List<IHookFilter<KeyboardStruct>>();

        public VKeys ModifierKeys { get; private set; }

        private IntPtr mHookHandle;
        private NativeMethods.HookProc mHookProc;
        private Dictionary<VKeys, DateTime> keyDict = new Dictionary<VKeys, DateTime>();

        ~KeyboardHook()
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
                (int)UnsafeNativeMethods.HookType.WH_KEYBOARD_LL,
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

        public IEnumerable<VKeys> GetDownedKeys()
        {
            return keyDict.Keys;
        }
        
        private IntPtr _Hook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var kb = Marshal.PtrToStructure(lParam, typeof(KeyboardStruct)) as KeyboardStruct;
                var msg = (int)wParam;
                var arg = new KeyboardHookEventArgs((VKeys)kb.vkCode);

                if (msg == NativeMethods.WM_KEYDOWN || msg == NativeMethods.WM_SYSKEYDOWN)
                {
                    if (!keyDict.ContainsKey(arg.KeyCode))
                    {
                        keyDict[arg.KeyCode] = DateTime.Now;

                        if (arg.ModifierKeys != VKeys.None)
                            this.ModifierKeys |= arg.ModifierKeys;

                        arg.IsFirst = true;
                    }

                    arg.ModifierKeys = this.ModifierKeys;

                    KeyDown?.Invoke(this, arg);
                }
                else if (msg == NativeMethods.WM_KEYUP || msg == NativeMethods.WM_SYSKEYUP)
                {
                    if (keyDict.ContainsKey(arg.KeyCode))
                    {
                        keyDict.Remove(arg.KeyCode);

                        if (arg.ModifierKeys != VKeys.None)
                            this.ModifierKeys &= ~arg.ModifierKeys;

                        arg.ModifierKeys = this.ModifierKeys;

                        KeyUp?.Invoke(this, arg);
                    }
                }

                if (arg.Handled)
                    return new IntPtr(1);
            }

            return UnsafeNativeMethods.CallNextHookEx(mHookHandle, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            UnHook();
        }
    }
}
