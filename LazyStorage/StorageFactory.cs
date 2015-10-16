using LazyStorage.InMemory;
using LazyStorage.Xml;

namespace LazyStorage
{
    public static class StorageFactory
    {
        public static IStorage GetInMemoryStorage()
        {
            return new InMemoryStorage();
        }

        public static IStorage GetXmlStorage(string storageFolder)
        {
            return new XmlStorage(storageFolder);
        }
    }
}