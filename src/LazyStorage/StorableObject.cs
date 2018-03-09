using System.Collections.Generic;

namespace LazyStorage
{
    public class StorableObject
    {
        public Dictionary<string, string> Info { get; }

        public StorableObject()
        {
            Info = new Dictionary<string, string>();
        }
    }
}