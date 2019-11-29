using System;
using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    internal class XmlStorage : IStorage
    {
        private readonly string _storageFolder;
        private readonly Dictionary<string, IRepository> _repos;

        public XmlStorage(string storageFolder)
        {
            _storageFolder = storageFolder;
            _repos = new Dictionary<string, IRepository>();
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                var xmlRepositoryWithConverter = new XmlRepository<T>(_storageFolder, converter);
                xmlRepositoryWithConverter.Load();
                _repos.Add(typeAsString, xmlRepositoryWithConverter);
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