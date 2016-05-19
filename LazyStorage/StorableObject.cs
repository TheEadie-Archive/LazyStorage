using System;
using System.Collections.Generic;

namespace LazyStorage
{
    public class StorableObject : IEquatable<StorableObject>
    {
        public int Id { get; set; }
        public Dictionary<string, string> Info { get; }

        public StorableObject()
        {
            Info = new Dictionary<string, string>();
        }
        
        public bool Equals(StorableObject other)
        {
            return (other.Id == Id);
        }
    }
}