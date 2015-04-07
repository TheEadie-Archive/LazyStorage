namespace LazyLibrary.Storage.Memory
{
    internal class MemoryStorage : IStorage
    {
        public IRepository<T> GetRepository<T>() where T : IStorable
        {
            return new MemoryRepository<T>();
        }

        public void Save()
        {
        }

        public void Discard()
        {
        }
    }
}