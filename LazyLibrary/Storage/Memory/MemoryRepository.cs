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
            return this.repository.Where(x => x.Id == id).SingleOrDefault();
        }

        public IQueryable<T> Get(System.Func<T, bool> exp)
        {
            return repository.Where(exp).AsQueryable<T>();
        }

        public void Upsert(T item)
        {
            var obj = GetById(item.Id);

            if (obj != null)
            {
                // Update
                this.repository.Remove(obj);
                this.repository.Add(item);
            }
            else
            {
                // Insert
                var nextId = this.repository.Max(x => x.Id) + 1;
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