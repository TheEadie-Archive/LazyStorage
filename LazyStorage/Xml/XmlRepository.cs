using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    internal class XmlRepository<T> : IRepository<T> where T : IStorable<T>, new()
    {
        private readonly string m_StorageFolder;
        private XDocument m_File;
        private readonly string m_Uri;

        public XmlRepository(string storageFolder)
        {
            m_StorageFolder = storageFolder;
            m_Uri = $"{storageFolder}{typeof(T)}.xml";
            Load();
        }

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            ICollection<T> found = new List<T>();

            foreach (var node in m_File.Element("Root").Elements())
            {
                var temp = new T();
                var info = new SerializationInfo(temp.GetType(), new FormatterConverter());
                
                foreach (var element in node.Descendants())
                {
                    info.AddValue(element.Name.ToString(), element.Value);
                }

                temp.InitialiseWithStorageInfo(info);

                found.Add(temp);
            }

            var query = found.AsQueryable<T>();
            return exp != null ? query.Where(exp).ToList() : found;
        }

        public void Set(T item)
        {
            var matchingItem = Get(x => x.Equals(item));

            if (matchingItem.Any())
            {
                Update(item);
            }
            else
            {
                Insert(item);
            }
        }

        private void Update(T item)
        {
            var info = item.GetStorageInfo();

            var rootElement = m_File.Element("Root");
            var idXElements = rootElement.Descendants("Id");
            var node = idXElements.SingleOrDefault(x => x.Value == item.Id.ToString());

            foreach (var data in info)
            {
                var asDateTime = (data.Value as DateTime?);
                if (asDateTime != null)
                {
                    node.Parent.Element(data.Name).Value = asDateTime.Value.ToString("s");
                    continue;
                }

                node.Parent.Element(data.Name).Value = data.Value.ToString();
            }
        }

        private void Insert(T item)
        {
            var typeAsString = typeof (T).ToString();

            var rootElement = m_File.Element("Root");
            var idXElements = rootElement.Descendants("Id");

            item.Id = idXElements.Any() ? idXElements.Max(x => (int) x) + 1 : 1;

            var info = item.GetStorageInfo();

            var newElement = new XElement(typeAsString);

            foreach (var data in info)
            {
                newElement.Add(new XElement(data.Name, data.Value));
            }

            rootElement.Add(newElement);
        }

        public void Delete(T item)
        {
            var rootElement = m_File.Element("Root");
            var idXElements = rootElement.Descendants("Id");
            var node = idXElements.SingleOrDefault(x => x.Value == item.Id.ToString());

            node = node.Parent;
            node.Remove();
        }

        public object Clone()
        {
            var newRepo = new XmlRepository<T>(m_StorageFolder);

            foreach (var item in Get())
            {
                var temp = new T();

                var info = item.GetStorageInfo();

                temp.InitialiseWithStorageInfo(info);

                newRepo.Set(temp);
            }

            return newRepo;
        }

        public void Save()
        {
            m_File.Save(m_Uri);
        }

        public void Load()
        {
            m_File = !File.Exists(m_Uri) ? new XDocument(new XElement("Root")) : XDocument.Load(m_Uri);
        }
    }
}