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
        private readonly string m_Uri;
        private List<T> m_Repository = new List<T>();
        private readonly string m_StorageFolder;
        private readonly IConverter<T> m_Converter;

        public JsonRepositoryWithConverter(string storageFolder, IConverter<T> converter)
        {
            m_Uri = $"{storageFolder}{typeof(T)}.json";
            m_StorageFolder = storageFolder;
            m_Converter = converter;
            Load();
        }

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            return exp != null ? m_Repository.Where(exp).ToList() : m_Repository.ToList();
        }

        public void Set(T item)
        {
            if (m_Repository.Contains(item))
            {
                // Update
                var storableObject = m_Converter.GetStorableObject(item);
                var obj = m_Repository.Where(x => m_Converter.IsEqual(storableObject, x));
                m_Repository.Remove(obj.First());
                m_Repository.Add(item);
            }
            else
            {
                // Insert
                m_Repository.Add(item);
            }
        }

        public void Delete(T item)
        {
            var storableObject = m_Converter.GetStorableObject(item);
            var obj = m_Repository.Where(x => m_Converter.IsEqual(storableObject, x));
            m_Repository.Remove(obj.First());
        }


        public object Clone()
        {
            var newRepo = new JsonRepositoryWithConverter<T>(m_StorageFolder, m_Converter);

            foreach (var item in Get())
            {
                var info = m_Converter.GetStorableObject(item);

                var temp = m_Converter.GetOriginalObject(info);

                newRepo.Set(temp);
            }

            return newRepo;

        }
        public void Load()
        {
            if (File.Exists(m_Uri))
            {
                var jsonContent = File.ReadAllText(m_Uri);
                var convertedItems = JsonConvert.DeserializeObject<List<StorableObject>>(jsonContent);

                m_Repository = convertedItems.Select(x => m_Converter.GetOriginalObject(x)).ToList();
            }
            else
            {
                m_Repository = new List<T>();
            }

        }

        public void Save()
        {
            var convertedItems = m_Repository.Select(item => m_Converter.GetStorableObject(item)).ToList();

            var fileContent = JsonConvert.SerializeObject(convertedItems, Formatting.Indented);
            File.WriteAllText(m_Uri, fileContent);
        }
    }
}