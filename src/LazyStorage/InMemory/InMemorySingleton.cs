using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    internal static class InMemorySingleton
    {
        private static Dictionary<string, IRepository> _repos = new Dictionary<string, IRepository>();

        public static void Sync<T>(string type, InMemoryRepository<T> repo) where T : IStorable<T>, new()
        {
            if (!_repos.ContainsKey(type))
            {
                _repos.Add(type, repo);
            }
            else
            {
                _repos[type] = (IRepository)repo.Clone();
            }
        }

        public static void Sync<T>(string type, InMemoryRepositoryWithConverter<T> repo)
        {
            if (!_repos.ContainsKey(type))
            {
                _repos.Add(type, repo);
            }
            else
            {
                _repos[type] = (IRepository)repo.Clone();
            }
        }

        public static IRepository<T> GetRepo<T>()
        {
            return _repos[nameof(T)];
        }

        public static void Clear()
        {
            _repos.Clear();
        }
    }
}