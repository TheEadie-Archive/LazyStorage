using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    internal class XmlRepository<T> : IRepository<T> where T : IStorable<T>, new()
    {
        private readonly string _uri;
        private List<T> _repository = new List<T>();

        public XmlRepository(string storageFolder)
        {
            _uri = $"{storageFolder}{typeof(T)}.xml";
            Load();
        }

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            return exp != null ? _repository.Where(exp).ToList() : _repository.ToList();
        }

        public void Set(T item)
        {
            if (_repository.Contains(item))
            {
                // Update
                var obj = _repository.Where(x => x.Equals(item));
                _repository.Remove(obj.First());
                _repository.Add(item);
            }
            else
            {
                // Insert
                var nextId = _repository.Any() ? _repository.Max(x => x.Id) + 1 : 1;
                item.Id = nextId;
                _repository.Add(item);
            }
        }

        public void Delete(T item)
        {
            var obj = _repository.SingleOrDefault(x => x.Id == item.Id);
            _repository.Remove(obj);
        }


        public void Load()
        {
            _repository = File.Exists(_uri) ? GetObjectsFromXml(_uri) : new List<T>();
        }

        public void Save()
        {
            using (var writer = new FileStream(_uri, FileMode.Create))
            {
                GetXmlOuput(_repository).Save(writer);
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