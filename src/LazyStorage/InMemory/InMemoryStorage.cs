using System;
using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    internal class InMemoryStorage : IStorage
    {
        private readonly Dictionary<string, IRepository> _repos;

        public InMemoryStorage()
        {
            _repos = new Dictionary<string, IRepository>();
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                var inMemoryRepositoryWithConverter = new InMemoryRepository<T>(converter);
                inMemoryRepositoryWithConverter.Load();
                _repos.Add(typeAsString, inMemoryRepositoryWithConverter);
            }

            if (!(_repos[typeAsString] is IRepository<T> repository))
            {
                throw new InvalidOperationException($"Unable to retrieve Repository for type {typeAsString}");
            }

            return repository;
        }

        public void Save()
        {
            foreach(var repo in _repos)
            {
                repo.Value.Save();
            }
        }

        public void Discard()
        {
            foreach(var repo in _repos)
            {
                repo.Value.Load();
            }
        }
    }
}