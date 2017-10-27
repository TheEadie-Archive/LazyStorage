using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LazyStorage.Interfaces;
using Newtonsoft.Json;

namespace LazyStorage.Json
{
    internal class JsonRepository<T> : IRepository<T> where T : IStorable<T>, new()
    {
        private readonly string _uri;
        private List<T> _repository = new List<T>();

        public JsonRepository(string storageFolder)
        {
            _uri = $"{storageFolder}{typeof(T)}.json";
            Load();
        }

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


        public object Clone()
        {
            var newRepo = new JsonRepository<T>(_uri);

            foreach (var item in Get())
            {
                var temp = new T();

                var info = item.GetStorageInfo();

                temp.InitialiseWithStorageInfo(info);

                newRepo.Set(temp);
            }

            return newRepo;
        }

        public void Load()
        {
            if (File.Exists(_uri))
            {
                var jsonContent = File.ReadAllText(_uri);
                _repository = JsonConvert.DeserializeObject<List<T>>(jsonContent);
            }
            else
            {
                _repository = new List<T>();
            }
            
        }

        public void Save()
        {
            var fileContent = JsonConvert.SerializeObject(_repository, Formatting.Indented);
            File.WriteAllText(_uri, fileContent);
        }
    }
}