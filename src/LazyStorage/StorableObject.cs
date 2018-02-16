using System.Collections.Generic;
using System.Reflection;

namespace LazyStorage
{
    public class StorableObject
    {
        public Dictionary<string, string> Info { get; }

        public StorableObject()
        {
            Info = new Dictionary<string, string>();
        }

        internal StorableObject(Dictionary<string, string> info)
        {
            Info = info;
        }
    }
}