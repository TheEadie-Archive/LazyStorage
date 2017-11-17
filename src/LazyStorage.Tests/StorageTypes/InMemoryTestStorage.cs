using LazyStorage.InMemory;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests.StorageTypes
{
    public class InMemoryTestStorage : ITestStorage
    {
        public IStorage GetStorage()
        {
            return StorageFactory.GetInMemoryStorage();
        }

        public void CleanUp()
        {
            InMemorySingleton.Clear();
        }
    }
}