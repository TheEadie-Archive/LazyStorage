using System;

namespace LazyStorage
{
    public interface IStorage
    {
        IRepository<T> GetRepository<T>() where T : IStorable<T>, new();
        IRepository<T> GetRepository<T>(IConverter<T> converter) where T : new();
        void Save();
        void Discard();
    }
}