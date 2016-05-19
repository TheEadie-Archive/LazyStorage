using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    internal class XmlRepositoryWithConverter<T> : IRepository<T>
    {
        private const string InternalIdString = "LazyStorageInternalId";
        private XDocument m_File;
        private readonly string m_Uri;
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
            ICollection<T> found = new List<T>();

            foreach (var node in m_File.Element("Root").Elements())
            {
                var storableObject = new StorableObject();

                foreach (var element in node.Descendants())
                {
                    storableObject.Info.Add(element.Name.ToString(), element.Value);
                }

                var temp = m_Converter.GetOriginalObject(storableObject);

                found.Add(temp);
            }

            var query = found.AsQueryable<T>();
            return exp != null ? query.Where(exp).ToList() : found;
        }

        private IEnumerable<StorableObject> GetMatchingItemsInStore(T item)
        {
            var found = new List<StorableObject>();

            foreach (var node in m_File.Element("Root").Elements())
            {
                var storableObject = new StorableObject();
                var idXElements = node.Descendants(InternalIdString);
                storableObject.LazyStorageInternalId = int.Parse(idXElements.Single().Value);
                foreach (var element in node.Descendants())
                {
                    storableObject.Info.Add(element.Name.ToString(), element.Value);
                }

                found.Add(storableObject);
            }
            return found.Where(x => m_Converter.IsEqual(x, item));
        }

        public void Set(T item)
        {
            var storableItem = m_Converter.GetStorableObject(item);

            var matchingItemsInStore = GetMatchingItemsInStore(item);

            if (matchingItemsInStore.Any())
            {
                Update(storableItem, matchingItemsInStore.First());
            }
            else
            {
                Insert(storableItem);
            }
        }

        private void Update(StorableObject item, StorableObject oldItem)
        {
            var info = item.Info;

            var rootElement = m_File.Element("Root");
            var idXElements = rootElement.Descendants(InternalIdString);
            var node = idXElements.Single(x => x.Value == oldItem.LazyStorageInternalId.ToString());

            foreach (var data in info)
            {
                node.Parent.Element(data.Key).Value = data.Value;
            }
        }

        private void Insert(StorableObject item)
        {
            var typeAsString = typeof (T).ToString();

            var rootElement = m_File.Element("Root");
            var idXElements = rootElement.Descendants(InternalIdString);

            item.LazyStorageInternalId = idXElements.Any() ? idXElements.Max(x => (int) x) + 1 : 1;

            var info = item.Info;

            var newElement = new XElement(typeAsString);
            newElement.Add(new XElement(InternalIdString, item.LazyStorageInternalId));

            foreach (var data in info)
            {
                newElement.Add(new XElement(data.Key, data.Value));
            }

            rootElement.Add(newElement);
        }

        public void Delete(T item)
        {
            var rootElement = m_File.Element("Root");
            var idXElements = rootElement.Descendants(InternalIdString);
            var node = idXElements.Single(x => x.Value == GetMatchingItemsInStore(item).First().LazyStorageInternalId.ToString());

            node = node.Parent;
            node.Remove();
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