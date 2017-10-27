namespace LazyStorage.Interfaces
{
    public interface IStorage
    {
        IRepository<T> GetRepository<T>() where T : IStorable<T>, new();
        IRepository<T> GetRepository<T>(IConverter<T> converter);
        void Save();
        void Discard();
    }
}