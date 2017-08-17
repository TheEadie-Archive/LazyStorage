using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    public class XmlRepository<T> : IRepository<T> where T : IStorable<T>, new()
    {
        private readonly string m_Uri;
        private List<T> m_Repository = new List<T>();

        public XmlRepository(string storageFolder)
        {
            m_Uri = $"{storageFolder}{typeof(T)}.xml";
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
            var newRepo = new XmlRepository<T>(m_Uri);

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
                m_Repository = GetObjectsFromXml(m_Uri);
            }
            else
            {
                m_Repository = new List<T>();
            }

        }

        public void Save()
        {
            using (var writer = new FileStream(m_Uri, FileMode.Create))
            {
                GetXmlOuput(m_Repository).Save(writer);
            }
        }

        private XDocument GetXmlOuput(List<T> objects)
        {
            var file = new XDocument(new XElement("Root"));

            var typeAsString = typeof(T).ToString();

            var rootElement = file.Element("Root");

            foreach (var item in objects)
            {
                var info = item.GetStorageInfo();

                var newElement = new XElement(typeAsString);
                
                foreach (var data in info)
                {
                    newElement.Add(new XElement(data.Key, data.Value));
                }

                rootElement.Add(newElement);
            }

            return file;
        }

        private List<T> GetObjectsFromXml(string uri)
        {
            var file = XDocument.Load(uri);

            var found = new List<T>();

            foreach (var node in file.Element("Root").Elements())
            {
                var storageInfo = new Dictionary<string, string>();

                foreach (var element in node.Descendants())
                {
                    storageInfo.Add(element.Name.ToString(), element.Value);
                }

                var item = new T();
                item.InitialiseWithStorageInfo(storageInfo);

                found.Add(item);
            }

            return found;
        }
    }
}