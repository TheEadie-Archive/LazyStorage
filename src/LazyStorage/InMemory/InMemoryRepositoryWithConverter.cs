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
        }

        private readonly List<StorableObject> _repository = new List<StorableObject>();
        
        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            var allObjects = _repository.Select(item => _converter.GetOriginalObject(item)).ToList();

            return exp != null ? allObjects.Where(exp).ToList() : allObjects.ToList();
        }

        public void Set(T item)
        {
            var storableItem = _converter.GetStorableObject(item);
            var matchingItemsInStore = _repository.Where(x => _converter.IsEqual(x, item));

            if (matchingItemsInStore.Any())
            {
                // Update
                _repository.Remove(matchingItemsInStore.First());
                _repository.Add(storableItem);
            }
            else
            {
                // Insert
                _repository.Add(storableItem);
            }
        }

        public void Delete(T item)
        {
            var storableItem = _converter.GetStorableObject(item);

            var obj = _repository.Where(x => _converter.IsEqual(storableItem, item));
            _repository.Remove(obj.First());
        }


        public object Clone()
        {
            var newRepo = new InMemoryRepositoryWithConverter<T>(_converter);

            foreach (var item in Get())
            {
                var info = _converter.GetStorableObject(item);

                var temp = _converter.GetOriginalObject(info);

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