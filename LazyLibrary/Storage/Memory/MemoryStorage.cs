using System.Collections.Generic;
namespace LazyLibrary.Storage.Memory
{
    internal class MemoryStorage : IStorage
    {
        private Dictionary<string, IRepository> repos = new Dictionary<string,IRepository>();

        public IRepository<T> GetRepository<T>() where T : IStorable
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
        }

        public void Discard()
        {
        }
    }
}