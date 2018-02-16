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

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var storableConverter = new StorableConverter<T>();
            return GetRepository(storableConverter);
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                var xmlRepositoryWithConverter = new XmlRepositoryWithConverter<T>(_storageFolder, converter);
                xmlRepositoryWithConverter.Load();
                _repos.Add(typeAsString, xmlRepositoryWithConverter);
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