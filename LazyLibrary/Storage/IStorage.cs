namespace LazyLibrary.Storage
{
    public interface IStorage
    {
        IRepository<T> GetRepository<T>() where T : IStorable;

        void Save();

        void Discard();
    }
}