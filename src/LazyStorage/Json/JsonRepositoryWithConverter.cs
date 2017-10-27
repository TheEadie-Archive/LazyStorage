using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LazyStorage.Interfaces;
using Newtonsoft.Json;

namespace LazyStorage.Json
{
    internal class JsonRepositoryWithConverter<T> : IRepository<T>
    {
        private readonly string _uri;
        private List<T> _repository = new List<T>();
        private readonly string _storageFolder;
        private readonly IConverter<T> _converter;

        public JsonRepositoryWithConverter(string storageFolder, IConverter<T> converter)
        {
            _uri = $"{storageFolder}{typeof(T)}.json";
            _storageFolder = storageFolder;
            _converter = converter;
            Load();
        }

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            return exp != null ? _repository.Where(exp).ToList() : _repository.ToList();
        }

        public void Set(T item)
        {
            var storableObject = _converter.GetStorableObject(item);
            var matchingItemsInStore = _repository.Where(x => _converter.IsEqual(storableObject, x));

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
            var storableObject = _converter.GetStorableObject(item);
            var obj = _repository.Where(x => _converter.IsEqual(storableObject, x));
            _repository.Remove(obj.First());
        }


        public object Clone()
        {
            var newRepo = new JsonRepositoryWithConverter<T>(_storageFolder, _converter);

            foreach (var item in Get())
            {
                var info = _converter.GetStorableObject(item);

                var temp = _converter.GetOriginalObject(info);

                newRepo.Set(temp);
            }

            return newRepo;

        }
        public void Load()
        {
            if (File.Exists(_uri))
            {
                var jsonContent = File.ReadAllText(_uri);
                var convertedItems = JsonConvert.DeserializeObject<List<StorableObject>>(jsonContent);

                _repository = convertedItems.Select(x => _converter.GetOriginalObject(x)).ToList();
            }
            else
            {
                _repository = new List<T>();
            }

        }

        public void Save()
        {
            var convertedItems = _repository.Select(item => _converter.GetStorableObject(item)).ToList();

            var fileContent = JsonConvert.SerializeObject(convertedItems, Formatting.Indented);
            File.WriteAllText(_uri, fileContent);
        }
    }
}