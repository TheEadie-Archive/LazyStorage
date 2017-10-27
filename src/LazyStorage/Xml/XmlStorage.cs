using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    internal class XmlStorage : IStorage
    {
        private readonly string _storageFolder;
        private Dictionary<string, IRepository> _repos;

        public XmlStorage(string storageFolder)
        {
            _storageFolder = storageFolder;
            _repos = new Dictionary<string, IRepository>();
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                _repos.Add(typeAsString, new XmlRepository<T>(_storageFolder));
            }

            return _repos[typeAsString] as IRepository<T>;
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!_repos.ContainsKey(typeAsString))
            {
                _repos.Add(typeAsString, new XmlRepositoryWithConverter<T>(_storageFolder, converter));
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
            _repos = new Dictionary<string, IRepository>();
        }
    }
}