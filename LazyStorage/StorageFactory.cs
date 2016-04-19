using LazyStorage.InMemory;
using LazyStorage.Interfaces;
using LazyStorage.Json;
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

        public static IStorage GetJsonStorage(string storageFolder)
        {
            return new JsonStorage(storageFolder);
        }
    }
}