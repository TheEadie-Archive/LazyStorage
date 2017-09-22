using LazyStorage.InMemory;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests.StorageTypes
{
    public class InMemoryTestStorage : ITestStorage
    {
        private readonly IStorage m_Storage = StorageFactory.GetInMemoryStorage();

        public IStorage GetStorage()
        {
            return m_Storage;
        }

        public void CleanUp()
        {
            InMemorySingleton.Clear();
        }
    }
}