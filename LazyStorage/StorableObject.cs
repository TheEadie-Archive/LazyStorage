using System;
using System.Runtime.Serialization;

namespace LazyStorage
{
    public class StorableObject : IEquatable<StorableObject>
    {
        public int Id { get; set; }
        public SerializationInfo Info { get; }

        public StorableObject()
        {
            Info = new SerializationInfo(GetType(), new FormatterConverter());
        }
        
        public bool Equals(StorableObject other)
        {
            return (other.Id == Id);
        }
    }
}