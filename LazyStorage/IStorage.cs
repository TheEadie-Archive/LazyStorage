namespace LazyStorage
{
    public interface IStorage
    {
        IRepository<T> GetRepository<T>() where T : IStorable<T>, new();
        void Save();
        void Discard();
    }
}