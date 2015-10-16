using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using LazyStorage.InMemory;

namespace LazyStorage.Xml
{
    internal class XmlStorage : IStorage
    {
        private XDocument m_File;
        private readonly string m_Uri;
        private Dictionary<string, IRepository> m_Repos;

        public XmlStorage(string storageFolder)
        {
            m_Uri = $"{storageFolder}LazyStorage.xml";
            m_File = !File.Exists(m_Uri) ? new XDocument(new XElement("Root")) : XDocument.Load(m_Uri);
            m_Repos = new Dictionary<string, IRepository>();
        }

        public IRepository<T> GetRepository<T>() where T : IStorable<T>, new()
        {
            var typeAsString = typeof(T).ToString();

            if (!m_Repos.ContainsKey(typeAsString))
            {
                m_Repos.Add(typeAsString, new XmlRepository<T>(m_File));
            }

            return m_Repos[typeAsString] as IRepository<T>;
        }

        public void Save()
        {
            m_File.Save(m_Uri);
        }

        public void Discard()
        {
            // Don't save
            m_File = !File.Exists(m_Uri) ? new XDocument(new XElement("Root")) : XDocument.Load(m_Uri);
            m_Repos = new Dictionary<string, IRepository>();
        }
    }
}