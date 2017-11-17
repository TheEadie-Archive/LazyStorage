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

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var typeAsString = typeof (T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                _repos.Add(typeAsString, new InMemoryRepository<T>());
            }

            return _repos[typeAsString] as IRepository<T>;
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                _repos.Add(typeAsString, new InMemoryRepositoryWithConverter<T>(converter));
            }

            return _repos[typeAsString] as IRepository<T>;
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