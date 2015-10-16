using LazyStorage.InMemory;

namespace LazyStorage.Tests.StorageTypes
{
    public class InMemoryTestStorage : ITestStorage
    {
        private readonly IStorage m_storage = StorageFactory.GetInMemoryStorage();

        public IStorage GetStorage()
        {
            return m_storage;
        }

        public void CleanUp()
        {
            InMemorySingleton.Clear();
        }
    }
}