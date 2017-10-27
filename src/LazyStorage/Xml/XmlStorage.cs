using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    internal class XmlStorage : IStorage
    {
        private readonly string m_StorageFolder;
        private Dictionary<string, IRepository> m_Repos;

        public XmlStorage(string storageFolder)
        {
            m_StorageFolder = storageFolder;
            m_Repos = new Dictionary<string, IRepository>();
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var typeAsString = typeof(T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new XmlRepository<T>(m_StorageFolder));
            }

            return m_Repos[typeAsString] as IRepository<T>;
        }

        public IRepository<T> GetRepository<T>(IConverter<T> converter)
        {
            var typeAsString = typeof(T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new XmlRepositoryWithConverter<T>(m_StorageFolder, converter));
            }

            return (IRepository<T>)m_Repos[typeAsString];
        }
        public void Save()
        {
            foreach (var repository in m_Repos)
            {
                repository.Value.Save();
            }
        }

        public void Discard()
        {
            foreach (var repository in m_Repos)
            {
                repository.Value.Load();
            }
            m_Repos = new Dictionary<string, IRepository>();
        }
    }
}