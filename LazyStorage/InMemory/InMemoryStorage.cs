using System;
using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    internal class InMemoryStorage : IStorage
    {
        private Dictionary<string, IRepository> m_Repos;

        public InMemoryStorage()
        {
            m_Repos = InMemorySingleton.GetRepo();
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var typeAsString = typeof (T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new InMemoryRepository<T>());
            }

            return (IRepository<T>) m_Repos[typeAsString];
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new InMemoryRepositoryWithConverter<T>(converter));
            }

            return (IRepository<T>)m_Repos[typeAsString];
        }

        public void Save()
        {
            InMemorySingleton.Sync(m_Repos);
        }

        public void Discard()
        {
            m_Repos = InMemorySingleton.GetRepo();
        }
    }
}