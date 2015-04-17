using System.Collections.Generic;
namespace LazyLibrary.Storage.Memory
{
    internal class MemoryStorage : IStorage
    {
        private Dictionary<string, IRepository> repos = new Dictionary<string,IRepository>();

        public MemoryStorage()
        {
            repos = MemorySingleton.GetRepo();
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>
        {
            string typeAsString = typeof(T).ToString();

            if (!repos.ContainsKey(typeAsString))
            {
                repos.Add(typeAsString, new MemoryRepository<T>());
            }

            return (IRepository<T>) repos[typeAsString];
        }

        public void Save()
        {
            MemorySingleton.Sync(repos);
        }

        public void Discard()
        {
        }
    }
}