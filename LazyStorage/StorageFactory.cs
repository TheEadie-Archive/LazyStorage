using System;
using LazyStorage.InMemory;

namespace LazyStorage
{
    public class StorageFactory
    {
        private IStorage m_Store;

        public IStorage GetInMemoryStorage()
        {
            if (m_Store == null)
            {
                m_Store = new InMemoryStorage();
            }

            return m_Store;
        }

        public IStorage GetXmlStorage(string storageFolder)
        {
            throw new NotImplementedException();
        }
    }
}