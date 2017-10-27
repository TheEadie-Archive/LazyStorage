using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.Json
{
    internal class JsonStorage : IStorage
    {
        private readonly string _storageFolder;
        private Dictionary<string, IRepository> _repos;

        public JsonStorage(string storageFolder)
        {
            _repos = new Dictionary<string, IRepository>();
            _storageFolder = storageFolder;
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var typeAsString = typeof (T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                _repos.Add(typeAsString, new JsonRepository<T>(_storageFolder));
            }

            return (IRepository<T>) _repos[typeAsString];
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                _repos.Add(typeAsString, new JsonRepositoryWithConverter<T>(_storageFolder, converter));
            }

            return (IRepository<T>)_repos[typeAsString];
        }

        public void Save()
        {
            foreach (var repository in _repos)
            {
                repository.Value.Save();
            }
        }

        public void Discard()
        {
            foreach (var repository in _repos)
            {
                repository.Value.Load();
            }
        }
    }
}