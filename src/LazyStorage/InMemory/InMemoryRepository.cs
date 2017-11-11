using System;
using System.Collections.Generic;
using System.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    internal class InMemoryRepository<T> : IRepository<T> where T : IStorable<T>, new()
    {
        private List<T> _repository = new List<T>();

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            return exp != null ? _repository.Where(exp).ToList() : _repository.ToList();
        }

        public void Set(T item)
        {
            if (_repository.Contains(item))
            {
                // Update
                var obj = _repository.Where(x => x.Equals(item));
                _repository.Remove(obj.First());
                _repository.Add(item);
            }
            else
            {
                // Insert
                var nextId = _repository.Any() ? _repository.Max(x => x.Id) + 1 : 1;
                item.Id = nextId;
                _repository.Add(item);
            }
        }

        public void Delete(T item)
        {
            var obj = _repository.SingleOrDefault(x => x.Id == item.Id);
            _repository.Remove(obj);
        }

        public void Save()
        {
            var itemsInRepo = Get().Select(x => x.GetStorageInfo());
            InMemorySingleton.Sync<T>(nameof(T), itemsInRepo);
        }

        public void Load()
        {
            _repository = InMemorySingleton.GetRepo<T>().Select(GetObjectFromStorageInfo).ToList();
        }

        private T GetObjectFromStorageInfo(Dictionary<string, string> storageInfo)
        {
            var item = new T();
            item.InitialiseWithStorageInfo(storageInfo);
            return item;
        }
    }
}