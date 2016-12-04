using ClusterFS;
using ClusterFS.IO;

namespace NextCapture.Database
{
    internal class KeySerializable : IClusterSerializable<string>
    {
        public bool CanDeserialize(ClusterStreamHolder holder)
        {
            return true;
        }

        public bool CanSerialize(string obj)
        {
            return true;
        }

        public string Deserialize(ClusterStreamHolder holder)
        {
            return holder.ReadString();
        }

        public void Serialize(string obj, ClusterStreamHolder holder)
        {
            holder.Write(obj);
        }
    }
}
