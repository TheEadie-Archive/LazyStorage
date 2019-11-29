using System;
using System.Collections.Generic;
using LazyStorage.Interfaces;

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

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                var jsonRepositoryWithConverter = new JsonRepository<T>(_storageFolder, converter);
                jsonRepositoryWithConverter.Load();
                _repos.Add(typeAsString, jsonRepositoryWithConverter);
            }

            if (!(_repos[typeAsString] is IRepository<T> repository))
            {
                throw new InvalidOperationException($"Unable to retrieve Repository for type {typeAsString}");
            }

            return repository;
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