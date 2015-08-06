using System;

namespace LazyStorage.Storage
{
    public interface IStorable<T> : IEquatable<T>
    {
        int Id { get; set; }
    }
}