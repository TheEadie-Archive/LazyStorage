using System.Collections.Generic;
using LazyStorage.Interfaces;
using LazyStorage.Xml;

namespace LazyStorage.Json
{
    internal class JsonStorage : IStorage
    {
        private readonly string _storageFolder;
        private readonly Dictionary<string, IRepository> _repos;

        public JsonStorage(string storageFolder)
        {
            _repos = new Dictionary<string, IRepository>();
            _storageFolder = storageFolder;
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            return GetRepository(new StorableConverter<T>());
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                var jsonRepositoryWithConverter = new JsonRepositoryWithConverter<T>(_storageFolder, converter);
                jsonRepositoryWithConverter.Load();
                _repos.Add(typeAsString, jsonRepositoryWithConverter);
            }

            return _repos[typeAsString] as IRepository<T>;
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