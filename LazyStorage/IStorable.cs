using System;
using System.Runtime.Serialization;
using System.Xml;

namespace LazyStorage
{
    public interface IStorable<T> : IEquatable<T>, ISerializable where T : new()
    {
        int Id { get; set; }
    }
}