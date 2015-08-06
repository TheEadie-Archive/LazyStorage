using System;
using System.Collections.Generic;
using System.Linq;

namespace LazyStorage.Storage.Memory
{
    internal class MemoryRepository<T> : IRepository<T> where T : IStorable<T>
    {
        private readonly List<T> m_Repository = new List<T>();

        public T GetById(int id)
        {
            return m_Repository.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<T> Get(Func<T, bool> exp = null)
        {
            return exp != null ? m_Repository.Where(exp).AsQueryable() : m_Repository.AsQueryable();
        }

        public void Upsert(T item)
        {
            if (m_Repository.Contains(item))
            {
                // Update
                var obj = m_Repository.Where(x => x.Equals(item));
                m_Repository.Remove(obj.First());
                m_Repository.Add(item);
            }
            else
            {
                // Insert
                var nextId = m_Repository.Any() ? m_Repository.Max(x => x.Id) + 1 : 1;
                item.Id = nextId;
                m_Repository.Add(item);
            }
        }

        public void Delete(T item)
        {
            var obj = GetById(item.Id);
            m_Repository.Remove(obj);
        }
    }
}