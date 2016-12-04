using ClusterFS;
using ClusterFS.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Collections;

namespace NextCapture.Database
{
    internal partial class CursorDB : IEnumerable<KeyValuePair<string, string>>
    {
        static string fileName = Path.Combine(DB.BaseDirectory, "cursors.cfs");

        StringPairSerializable dictSerializer;
        KeySerializable keySerializer;
        CFSFile db;

        public string this[string key]
        {
            get
            {
                return db
                    .GetItems(dictSerializer)
                    .FirstOrDefault(kv => kv.Key == key).Value;
            }
            set
            {
                Update(key, value);
            }
        }

        public int Count
        {
            get
            {
                return db.GetItems(keySerializer).Count();
            }
        }

        public CursorDB()
        {
            db = DB.Open(fileName, 256);

            dictSerializer = new StringPairSerializable();
            keySerializer = new KeySerializable();
        }

        public void Add(string key, string value)
        {
            var kv = new KeyValuePair<string, string>(key, value);

            db.AddItem(kv, dictSerializer, true);
        }

        public void Remove(string key)
        {
            db.RemoveCluster(GetStreamHolder(key).Index, Architecture.Logical);
        }

        public void Clear()
        {
            foreach (var c in db.AllClusters(Architecture.Physical))
                db.RemoveCluster(c.Index, Architecture.Physical);
        }

        private void Update(string key, string value)
        {
            var holder = GetStreamHolder(key);

            if (holder != null)
            {
                holder.Seek(0);

                dictSerializer.Serialize(
                    new KeyValuePair<string, string>(key, value), 
                    holder);
            }
            else
            {
                Add(key, value);
            }
        }

        private ClusterStreamHolder GetStreamHolder(string key)
        {
            var holder = db
                .AllClusters(Architecture.Logical)
                .FirstOrDefault(h => keySerializer.Deserialize(h) == key);

            return holder;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return db
                .GetItems(dictSerializer)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}