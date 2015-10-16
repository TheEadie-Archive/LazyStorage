using System;
using System.Collections.Generic;

namespace LazyStorage
{
    public interface IRepository<T> : IRepository
    {
        T GetById(int id);
        ICollection<T> Get(Func<T, bool> exp = null);
        void Upsert(T item);
        void Delete(T item);
    }

    public interface IRepository : ICloneable
    {
    }
}