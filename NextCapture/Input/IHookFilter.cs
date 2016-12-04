using System;

namespace NextCapture.Input
{
    public interface IHookFilter<T>
    {
        bool HookProc(IntPtr wParam, IntPtr lParam, T data);
    }
}
