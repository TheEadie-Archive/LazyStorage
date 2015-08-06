using System.Collections.Generic;

namespace LazyStorage.Memory
{
    internal class MemoryStorage : IStorage
    {
        private readonly Dictionary<string, IRepository> m_Repos;

        public MemoryStorage()
        {
            m_Repos = MemorySingleton.GetRepo();
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>
        {
            var typeAsString = typeof (T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new MemoryRepository<T>());
            }

            return (IRepository<T>) m_Repos[typeAsString];
        }

        public void Save()
        {
            MemorySingleton.Sync(m_Repos);
        }

        public void Discard()
        {
        }
    }
}