﻿using System;
using System.Collections.Generic;

namespace LazyStorage.Interfaces
{
    public interface IRepository<T> : IRepository
    {
        ICollection<T> Get(Func<T, bool> exp = null);
        void Set(T item);
        void Delete(T item);
    }

    public interface IRepository : ICloneable
    {
        void Save();
        void Load();
    }
}