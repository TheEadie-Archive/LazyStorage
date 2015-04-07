using System.Collections.Generic;
using System.Linq;

namespace LazyLibrary.Storage.Memory
{
    internal class MemoryRepository<T> : IRepository<T> where T : IStorable
    {
        private Dictionary<int, T> repository = new Dictionary<int, T>();

        public T GetById(int id)
        {
            return this.repository.ContainsKey(id) ? this.repository[id] : default(T);
        }

        public void Insert(T item)
        {
            int nextId = this.repository.Keys.Max() + 1;
            item.Id = nextId;
            this.repository.Add(item.Id, item);
        }

        public void Update(T item)
        {
            this.repository.Remove(item.Id);
            this.repository.Add(item.Id, item);
        }

        public void Delete(T item)
        {
            this.repository.Remove(item.Id);
        }
    }
}