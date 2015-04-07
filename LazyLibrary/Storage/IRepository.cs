using System.Linq;
namespace LazyLibrary.Storage
{
    public interface IRepository<T>
    {
        T GetById(int id);

        IQueryable<T> Get();

        void Upsert(T item);

        void Delete(T item);
    }
}