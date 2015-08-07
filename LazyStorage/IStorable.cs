using System;
using System.Runtime.Serialization;

namespace LazyStorage
{
    public interface IStorable<T> : IEquatable<T>, ISerializable
    {
        int Id { get; set; }
    }
}