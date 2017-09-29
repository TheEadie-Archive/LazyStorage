using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    public class XmlRepositoryWithConverter<T> : IRepository<T>
    {
        private readonly string m_Uri;
        private List<T> m_Repository = new List<T>();
        private readonly string m_StorageFolder;
        private readonly IConverter<T> m_Converter;

        public XmlRepositoryWithConverter(string storageFolder, IConverter<T> converter)
        {
            m_Uri = $"{storageFolder}{typeof(T)}.xml";
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
            var storableObject = m_Converter.GetStorableObject(item);
            var matchingItemsInStore = m_Repository.Where(x => m_Converter.IsEqual(storableObject, x));

            if (matchingItemsInStore.Any())
            {
                // Update
                m_Repository.Remove(matchingItemsInStore.First());
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
            var newRepo = new XmlRepositoryWithConverter<T>(m_StorageFolder, m_Converter);

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
                var info = m_Converter.GetStorableObject(item).Info;

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
                var storableObject = new StorableObject();

                foreach (var element in node.Descendants())
                {
                    storableObject.Info.Add(element.Name.ToString(), element.Value);
                }

                var item = m_Converter.GetOriginalObject(storableObject);

                found.Add(item);
            }

            return found;
        }
    }
}