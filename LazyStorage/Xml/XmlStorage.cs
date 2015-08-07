using System;
using System.Collections.Generic;
using LazyStorage.InMemory;

namespace LazyStorage.Xml
{
    public class XmlStorage : IStorage
    {
        private readonly string m_StorageFolder;
        private readonly Dictionary<string, IRepository> m_Repos;

        public XmlStorage(string storageFolder)
        {
            m_StorageFolder = storageFolder;
            m_Repos = new Dictionary<string, IRepository>();
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>
        {
            var typeAsString = typeof(T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new XmlRepository<T>(m_StorageFolder));
            }

            return m_Repos[typeAsString] as IRepository<T>;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Discard()
        {
            throw new NotImplementedException();
        }
    }
}