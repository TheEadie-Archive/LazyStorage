using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace LazyStorage.Xml
{
    internal class XmlRepositoryWithConverter<T> : IRepository<T> where T : new()
    {
        private XDocument XmlFile { get; set; }
        private readonly IConverter<T> m_Converter;

        public XmlRepositoryWithConverter(XDocument file, IConverter<T> converter)
        {
            XmlFile = file;
            m_Converter = converter;
        }

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            ICollection<T> found = new List<T>();

            foreach (var node in XmlFile.Element("Root").Elements())
            {
                var storableObject = new StorableObject();

                foreach (var element in node.Descendants())
                {
                    storableObject.Info.AddValue(element.Name.ToString(), element.Value);
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

            foreach (var node in XmlFile.Element("Root").Elements())
            {
                var storableObject = new StorableObject();
                var idXElements = node.Descendants("Id");
                storableObject.Id = int.Parse(idXElements.Single().Value);
                foreach (var element in node.Descendants())
                {
                    storableObject.Info.AddValue(element.Name.ToString(), element.Value);
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

            var rootElement = XmlFile.Element("Root");
            var idXElements = rootElement.Descendants("Id");
            var node = idXElements.Single(x => x.Value == oldItem.Id.ToString());

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

        private void Insert(StorableObject item)
        {
            var typeAsString = typeof (T).ToString();

            var rootElement = XmlFile.Element("Root");
            var idXElements = rootElement.Descendants("Id");

            item.Id = idXElements.Any() ? idXElements.Max(x => (int) x) + 1 : 1;

            var info = item.Info;

            var newElement = new XElement(typeAsString);
            newElement.Add(new XElement("Id", item.Id));

            foreach (var data in info)
            {
                newElement.Add(new XElement(data.Name, data.Value));
            }

            rootElement.Add(newElement);
        }

        public void Delete(T item)
        {
            var rootElement = XmlFile.Element("Root");
            var idXElements = rootElement.Descendants("Id");
            var node = idXElements.Single(x => x.Value == GetMatchingItemsInStore(item).First().Id.ToString());

            node = node.Parent;
            node.Remove();
        }

        public object Clone()
        {
            var newRepo = new XmlRepositoryWithConverter<T>(XmlFile, m_Converter);

            foreach (var item in Get())
            {
                var info = m_Converter.GetStorableObject(item);

                var temp = m_Converter.GetOriginalObject(info);

                newRepo.Set(temp);
            }

            return newRepo;

        }
    }
}