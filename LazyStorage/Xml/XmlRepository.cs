using System;
using System.Linq;
using System.Xml.Linq;

namespace LazyStorage.Xml
{
    public class XmlRepository<T> : IRepository<T>
    {
        private XDocument File { get; set; }

        public XmlRepository(XDocument file)
        {
            File = file;
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
            throw new NotImplementedException();
        }

        public void Delete(T item)
        {
            throw new NotImplementedException();
        }
    }
}