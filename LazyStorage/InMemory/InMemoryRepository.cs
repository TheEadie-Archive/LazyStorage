using System;
using System.Collections.Generic;
using System.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    internal class InMemoryRepository<T> : IRepository<T> where T : IStorable<T>, new()
    {
        private readonly List<T> m_Repository = new List<T>();

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            return exp != null ? m_Repository.Where(exp).ToList() : m_Repository.ToList();
        }

        public void Set(T item)
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
            var obj = m_Repository.SingleOrDefault(x => x.Id == item.Id);
            m_Repository.Remove(obj);
        }


        public object Clone()
        {
            var newRepo = new InMemoryRepository<T>();

            foreach (var item in Get())
            {
                var temp = new T();

                var info = item.GetStorageInfo();

                temp.InitialiseWithStorageInfo(info);

                newRepo.Set(temp);
            }

            return newRepo;
        }
    }
}