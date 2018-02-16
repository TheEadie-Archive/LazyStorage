namespace LazyStorage.Interfaces
{
    public interface IStorage
    {
        IRepository<T> GetRepository<T>(IConverter<T> converter);
        void Save();
        void Discard();
    }
}