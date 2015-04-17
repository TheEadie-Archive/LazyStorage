﻿namespace LazyLibrary.Storage
{
    public interface IStorage
    {
        IRepository<T> GetRepository<T>() where T : IStorable<T>;

        void Save();

        void Discard();
    }
}