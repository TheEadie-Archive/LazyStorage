using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    internal class InMemoryStorage : IStorage
    {
        private Dictionary<string, IRepository> _repos;

        public InMemoryStorage()
        {
            _repos = InMemorySingleton.GetRepo();
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var typeAsString = typeof (T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                _repos.Add(typeAsString, new InMemoryRepository<T>());
            }

            return (IRepository<T>) _repos[typeAsString];
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                _repos.Add(typeAsString, new InMemoryRepositoryWithConverter<T>(converter));
            }

            return (IRepository<T>)_repos[typeAsString];
        }

        public void Save()
        {
            InMemorySingleton.Sync(_repos);
        }

        public void Discard()
        {
            _repos = InMemorySingleton.GetRepo();
        }
    }
}