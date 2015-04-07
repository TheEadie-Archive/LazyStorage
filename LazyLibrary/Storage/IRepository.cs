namespace LazyLibrary.Storage
{
    public interface IRepository<T>
    {
        T GetById(int id);

        void Insert(T item);

        void Update(T item);

        void Delete(T item);
    }
}