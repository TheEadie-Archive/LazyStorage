using LazyLibrary.Storage.Memory;

namespace LazyLibrary.Storage
{
    public class StorageFactory
    {
        private IStorage store;

        public StorageFactory()
        {
        }

        public IStorage GetStorage()
        {
            if (this.store == null)
            {
                this.store = new MemoryStorage();
            }

            return this.store;
        }
    }
}