using ClusterFS;

namespace NextCapture.Database
{
    internal partial class CursorDB
    {
        class StringPairSerializable : KeyValuePairSerializable<string>
        {
            public override string DeserializeValue(ClusterStreamHolder holder)
            {
                return holder.ReadString();
            }

            public override void SerializeValue(string value, ClusterStreamHolder holder)
            {
                holder.Write(value);
            }
        }
    }
}