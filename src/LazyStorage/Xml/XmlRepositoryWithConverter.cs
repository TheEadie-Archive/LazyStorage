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
        private readonly string _uri;
        private List<T> _repository = new List<T>();
        private readonly string _storageFolder;
        private readonly IConverter<T> _converter;

        public XmlRepositoryWithConverter(string storageFolder, IConverter<T> converter)
        {
            _uri = $"{storageFolder}{typeof(T)}.xml";
            _storageFolder = storageFolder;
            _converter = converter;
            Load();
        }

        public ICollection<T> Get(Func<T, bool> exp = null)
        {
            return exp != null ? _repository.Where(exp).ToList() : _repository.ToList();
        }

        public void Set(T item)
        {
            var storableObject = _converter.GetStorableObject(item);
            var matchingItemsInStore = _repository.Where(x => _converter.IsEqual(storableObject, x));

            if (matchingItemsInStore.Any())
            {
                // Update
                _repository.Remove(matchingItemsInStore.First());
                _repository.Add(item);
            }
            else
            {
                // Insert
                _repository.Add(item);
            }
        }

        public void Delete(T item)
        {
            var storableObject = _converter.GetStorableObject(item);
            var obj = _repository.Where(x => _converter.IsEqual(storableObject, x));
            _repository.Remove(obj.First());
        }


        public object Clone()
        {
            var newRepo = new XmlRepositoryWithConverter<T>(_storageFolder, _converter);

            foreach (var item in Get())
            {
                var info = _converter.GetStorableObject(item);

                var temp = _converter.GetOriginalObject(info);

                newRepo.Set(temp);
            }

            return newRepo;

        }
        public void Load()
        {
            if (File.Exists(_uri))
            {
                _repository = GetObjectsFromXml(_uri);
            }
            else
            {
                _repository = new List<T>();
            }

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
                var info = _converter.GetStorableObject(item).Info;

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

                var item = _converter.GetOriginalObject(storableObject);

                found.Add(item);
            }

            return found;
        }
    }
}