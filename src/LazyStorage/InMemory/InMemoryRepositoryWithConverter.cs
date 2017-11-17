using System;
using System.Collections.Generic;
using System.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    internal class InMemoryRepositoryWithConverter<T> : IRepository<T>
    {
        private readonly IConverter<T> _converter;

        public InMemoryRepositoryWithConverter(IConverter<T> converter)
        {
            _converter = converter;
            Load();
        }

        private List<T> _repository = new List<T>();
        
        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            var allObjects = _repository;

            return exp != null ? allObjects.Where(exp).ToList() : allObjects.ToList();
        }

        public void Set(T item)
        {
            var storableItem = _converter.GetStorableObject(item);
            var matchingItemsInStore = _repository.Where(x => _converter.IsEqual(storableItem, x)).ToList();

            if (matchingItemsInStore.Any())
            {
                // Update
                _repository.Remove(matchingItemsInStore.First());
                _repository.Add(item);
            }
            else
            {
                // Insert
                _repository.Add(item);
            }
        }

        public void Delete(T item)
        {
            var storableItem = _converter.GetStorableObject(item);

            var obj = _repository.Where(x => _converter.IsEqual(storableItem, item));
            _repository.Remove(obj.First());
        }

        public void Save()
        {
            var itemsInRepo = Get().Select(x => _converter.GetStorableObject(x).Info);
            InMemorySingleton.Sync<T>(nameof(T), itemsInRepo);
        }

        public void Load()
        {
            _repository = InMemorySingleton.GetRepo<T>().Select(GetObjectFromStorageInfo).ToList();
        }

        private T GetObjectFromStorageInfo(Dictionary<string, string> storageInfo)
        {
            var storableItem = new StorableObject();

            foreach(var itemInfo in storageInfo)
            {
                storableItem.Info.Add(itemInfo.Key, itemInfo.Value);
            }

            var item = _converter.GetOriginalObject(storableItem);
            
            return item;
        }
    }
}