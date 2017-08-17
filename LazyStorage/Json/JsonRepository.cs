using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LazyStorage.Interfaces;
using Newtonsoft.Json;

namespace LazyStorage.Json
{
    public class JsonRepository<T> : IRepository<T> where T : IStorable<T>, new()
    {
        private readonly string m_Uri;
        private List<T> m_Repository = new List<T>();

        public JsonRepository(string storageFolder)
        {
            m_Uri = $"{storageFolder}{typeof(T)}.json";
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
            var newRepo = new JsonRepository<T>(m_Uri);

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
            if (File.Exists(m_Uri))
            {
                var jsonContent = File.ReadAllText(m_Uri);
                m_Repository = JsonConvert.DeserializeObject<List<T>>(jsonContent);
            }
            else
            {
                m_Repository = new List<T>();
            }
            
        }

        public void Save()
        {
            var fileContent = JsonConvert.SerializeObject(m_Repository, Formatting.Indented);
            File.WriteAllText(m_Uri, fileContent);
        }
    }
}