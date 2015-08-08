using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace LazyStorage.Xml
{
    public class XmlRepository<T> : IRepository<T> where T : IStorable<T>, new()
    {
        private XDocument XmlFile { get; set; }

        public XmlRepository(XDocument file)
        {
            XmlFile = file;
        }

        public T GetById(int id)
        {
            return Get(x => x.Id == id).SingleOrDefault();
        }

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            ICollection<T> found = new List<T>();

            foreach (var node in XmlFile.Element("Root").Elements())
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

        public void Upsert(T item)
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

            var rootElement = XmlFile.Element("Root");
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

            var rootElement = XmlFile.Element("Root");
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
            throw new NotImplementedException();
        }
    }
}