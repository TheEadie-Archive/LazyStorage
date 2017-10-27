using LazyStorage.InMemory;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests.StorageTypes
{
    public class InMemoryTestStorage : ITestStorage
    {
        private readonly IStorage _storage = StorageFactory.GetInMemoryStorage();

        public IStorage GetStorage()
        {
            return _storage;
        }

        public void CleanUp()
        {
            InMemorySingleton.Clear();
        }
    }
}