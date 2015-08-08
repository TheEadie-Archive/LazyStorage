﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using LazyStorage.InMemory;

namespace LazyStorage.Xml
{
    public class XmlStorage : IStorage
    {
        private readonly XDocument m_File;
        private readonly string m_Uri;
        private readonly Dictionary<string, IRepository> m_Repos;

        public XmlStorage(string storageFolder)
        {
            m_Uri = string.Format("{0}LazyStorage.xml", storageFolder);
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
            throw new NotImplementedException();
        }
    }
}