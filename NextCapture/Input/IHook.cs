using System.Collections.Generic;

namespace NextCapture.Input
{
    public interface IHook<T>
    {
        IList<IHookFilter<T>> Filters { get; }

        bool IsHooked { get; }

        bool Hook();
        bool UnHook();
    }
}
