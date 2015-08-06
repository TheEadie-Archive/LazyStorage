using System;

namespace LazyStorage
{
    public interface IStorable<T> : IEquatable<T>
    {
        int Id { get; set; }
    }
}