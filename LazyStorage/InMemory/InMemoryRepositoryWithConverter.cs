using System;
using System.Collections.Generic;
using System.Linq;

namespace LazyStorage.InMemory
{
    internal class InMemoryRepositoryWithConverter<T> : IRepository<T> where T : IEquatable<T>, new()
    {
        private readonly IConverter<T> m_Converter;

        public InMemoryRepositoryWithConverter(IConverter<T> converter)
        {
            m_Converter = converter;
        }

        private readonly List<StorableObject> m_Repository = new List<StorableObject>();

        public T GetById(int id)
        {
            return m_Converter.GetOriginalObject(m_Repository.SingleOrDefault(x => x.Id == id));
        }

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            var allObjects = m_Repository.Select(item => m_Converter.GetOriginalObject(item)).ToList();

            return exp != null ? allObjects.Where(exp).ToList() : allObjects.ToList();
        }

        public void Upsert(T item)
        {
            var allObjects = m_Repository.Select(i => m_Converter.GetOriginalObject(i)).ToList();
            var storableItem = m_Converter.GetStorableObject(item);

            if (allObjects.Contains(item))
            {
                // Update
                var obj = m_Repository.Where(x => m_Converter.IsEqual(storableItem, item));
                m_Repository.Remove(obj.First());
                m_Repository.Add(storableItem);
            }
            else
            {
                // Insert
                var nextId = m_Repository.Any() ? m_Repository.Max(x => x.Id) + 1 : 1;
                storableItem.Id = nextId;
                m_Repository.Add(storableItem);
            }
        }

        public void Delete(T item)
        {
            var storableItem = m_Converter.GetStorableObject(item);

            var obj = m_Repository.Where(x => m_Converter.IsEqual(storableItem, item));
            m_Repository.Remove(obj.First());
        }


        public object Clone()
        {
            var newRepo = new InMemoryRepositoryWithConverter<T>(m_Converter);

            foreach (var item in Get())
            {
                var info = m_Converter.GetStorableObject(item);

                var temp = m_Converter.GetOriginalObject(info);

                newRepo.Upsert(temp);
            }

            return newRepo;
        }
    }
}