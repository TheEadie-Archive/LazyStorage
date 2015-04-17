using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LazyLibrary.Storage.Memory
{
    internal class MemoryRepository<T> : IRepository<T> where T : IStorable
    {
        private List<T> repository = new List<T>();

        public T GetById(int id)
        {
            return this.repository.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<T> Get(System.Func<T, bool> exp = null)
        {
            return exp != null ? repository.Where(exp).AsQueryable<T>() : repository.AsQueryable<T>();
        }

        public void Upsert(T item)
        {
            if (repository.Contains(item))
            {
                // Update
                var obj = repository.Single(x => x.Equals(item));
                this.repository.Remove(obj);
                this.repository.Add(item);
            }
            else
            {
                // Insert
                var nextId = this.repository.Any() ? this.repository.Max(x => x.Id) + 1 : 1;
                item.Id = nextId;
                this.repository.Add(item);
            }
        }

        public void Delete(T item)
        {
            var obj = GetById(item.Id);
            this.repository.Remove(obj);
        }
    }
}