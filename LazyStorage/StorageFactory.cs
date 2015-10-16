using LazyStorage.InMemory;
using LazyStorage.Xml;

namespace LazyStorage
{
    public class StorageFactory
    {
        public IStorage GetInMemoryStorage()
        {
            return new InMemoryStorage();
        }

        public IStorage GetXmlStorage(string storageFolder)
        {
            return new XmlStorage(storageFolder);
        }
    }
}