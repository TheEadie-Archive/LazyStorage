using System;
using System.Runtime.Serialization;

namespace LazyStorage.Interfaces
{
    public interface IStorable<T> : IEquatable<T> where T : new()
    {
        int Id { get; set; }
        SerializationInfo GetStorageInfo();
        void InitialiseWithStorageInfo(SerializationInfo info);
    }
}