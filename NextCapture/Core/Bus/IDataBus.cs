namespace NextCapture.Core
{
    interface IDataBus<T>
    {
        bool Enabled { get; set; }

        bool SendData(T data);

        bool OnSendData(T data);
    }
}
