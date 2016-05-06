using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.Json
{
    internal class JsonStorage : IStorage
    {
        private readonly string m_StorageFolder;
        private Dictionary<string, IRepository> m_Repos;

        public JsonStorage(string storageFolder)
        {
            m_Repos = new Dictionary<string, IRepository>();
            m_StorageFolder = storageFolder;
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var typeAsString = typeof (T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new JsonRepository<T>(m_StorageFolder));
            }

            return (IRepository<T>) m_Repos[typeAsString];
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new JsonRepositoryWithConverter<T>(m_StorageFolder, converter));
            }

            return (IRepository<T>)m_Repos[typeAsString];
        }

        public void Save()
        {
            foreach (var repository in m_Repos)
            {
                repository.Value.Save();
            }
        }

        public void Discard()
        {
            foreach (var repository in m_Repos)
            {
                repository.Value.Load();
            }
        }
    }
}