using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace LazyStorage.Xml
{
    public class XmlRepository<T> : IRepository<T> where T : IStorable<T>
    {
        private XDocument XmlFile { get; set; }

        public XmlRepository(XDocument file)
        {
            XmlFile = file;
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(Func<T, bool> exp = null)
        {
            throw new NotImplementedException();
        }

        public void Upsert(T item)
        {
            var typeAsString = typeof(T).ToString();

            var rootElement = XmlFile.Element("Root");
            var idXElements = rootElement.Descendants("Id");

            item.Id = idXElements.Any() ? idXElements.Max(x => (int) x) + 1 : 1;

            var serializationInfo = new SerializationInfo(item.GetType(), new FormatterConverter());
            item.GetObjectData(serializationInfo, new StreamingContext());

            var newElement = new XElement(typeAsString);

            foreach (var data in serializationInfo)
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