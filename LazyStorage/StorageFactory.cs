using LazyStorage.InMemory;
using LazyStorage.Xml;

namespace LazyStorage
{
    public class StorageFactory
    {
        private IStorage m_Store;

        public IStorage GetInMemoryStorage()
        {
            return m_Store = new InMemoryStorage();
        }

        public IStorage GetXmlStorage(string storageFolder)
        {
            return new XmlStorage(storageFolder);
        }
    }
}