using System;
using System.Collections;
using System.Text;
using System.Runtime.Serialization;

namespace DY.Common
{
    /// <summary>
    /// 名称：无排序，不区分大小写 HashTable
    /// </summary>
    [Serializable]
    public class NBHashTable : Hashtable {
        private ArrayList keys = new ArrayList();

        public NBHashTable():base(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default) { }
        public NBHashTable(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void Add(object key, object value) {
            base.Add(key, value);
            keys.Add(key);
        }

        public override ICollection Keys {
            get {
                return keys;
            }
        }

        public override void Clear() {
            base.Clear();
            keys.Clear();
        }

        public override void Remove(object key) {
            base.Remove(key);
            keys.Remove(key);
        }
        public override IDictionaryEnumerator GetEnumerator() {
            return base.GetEnumerator();
        }
    }
}
