using ClusterFS;
using ClusterFS.IO;
using System.Collections.Generic;

namespace NextCapture.Database
{
    internal abstract class KeyValuePairSerializable<TValue> : IClusterSerializable<KeyValuePair<string, TValue>>
    {
        public bool CanDeserialize(ClusterStreamHolder holder)
        {
            return holder != null;
        }

        public bool CanSerialize(KeyValuePair<string, TValue> obj)
        {
            return true;
        }

        public KeyValuePair<string, TValue> Deserialize(ClusterStreamHolder holder)
        {
            return new KeyValuePair<string, TValue>(
                holder.ReadString(),
                DeserializeValue(holder));
        }
        
        public void Serialize(KeyValuePair<string, TValue> obj, ClusterStreamHolder holder)
        {
            holder.Write(obj.Key);
            SerializeValue(obj.Value, holder);
        }

        public abstract TValue DeserializeValue(ClusterStreamHolder holder);
        public abstract void SerializeValue(TValue value, ClusterStreamHolder holder);
    }
}
