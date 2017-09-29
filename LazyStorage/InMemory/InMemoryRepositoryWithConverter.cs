using System;
using System.Collections.Generic;
using System.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    public class InMemoryRepositoryWithConverter<T> : IRepository<T>
    {
        private readonly IConverter<T> m_Converter;

        public InMemoryRepositoryWithConverter(IConverter<T> converter)
        {
            m_Converter = converter;
        }

        private readonly List<StorableObject> m_Repository = new List<StorableObject>();
        
        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            var allObjects = m_Repository.Select(item => m_Converter.GetOriginalObject(item)).ToList();

            return exp != null ? allObjects.Where(exp).ToList() : allObjects.ToList();
        }

        public void Set(T item)
        {
            var storableItem = m_Converter.GetStorableObject(item);
            var matchingItemsInStore = m_Repository.Where(x => m_Converter.IsEqual(x, item));

            if (matchingItemsInStore.Any())
            {
                // Update
                m_Repository.Remove(matchingItemsInStore.First());
                m_Repository.Add(storableItem);
            }
            else
            {
                // Insert
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

                newRepo.Set(temp);
            }

            return newRepo;
        }

        public void Save()
        {
        }

        public void Load()
        {
        }
    }
}