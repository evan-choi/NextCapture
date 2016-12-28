using System;

namespace NextCapture
{
    class ValueChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; set; }
        public T NewValue { get; set; }

        public ValueChangedEventArgs()
        {
        }

        public ValueChangedEventArgs(T old, T @new)
        {
            this.OldValue = old;
            this.NewValue = @new;
        }
    }
}
