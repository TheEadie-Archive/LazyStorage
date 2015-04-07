using System.Linq;
namespace LazyLibrary.Storage
{
    public interface IRepository<T> : IRepository
    {
        T GetById(int id);

        IQueryable<T> Get(System.Func<T, bool> exp);

        void Upsert(T item);

        void Delete(T item);
    }

    public interface IRepository
    {
    }
}