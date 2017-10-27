using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    internal static class InMemorySingleton
    {
        private static Dictionary<string, IRepository> _repos = new Dictionary<string, IRepository>();

        public static void Sync(Dictionary<string, IRepository> repoList)
        {
            _repos = new Dictionary<string, IRepository>();

            foreach (var repo in repoList)
            {
                _repos.Add(repo.Key, (IRepository)repo.Value.Clone());
            }
        }

        public static Dictionary<string, IRepository> GetRepo()
        {
            return _repos;
        }

        public static void Clear()
        {
            _repos.Clear();
        }
    }
}