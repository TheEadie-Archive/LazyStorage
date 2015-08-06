using System;
using System.Linq;

namespace LazyStorage.Storage
{
    public interface IRepository<T> : IRepository
    {
        T GetById(int id);
        IQueryable<T> Get(Func<T, bool> exp = null);
        void Upsert(T item);
        void Delete(T item);
    }

    public interface IRepository
    {
    }
}