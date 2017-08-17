using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.InMemory
{
    public static class InMemorySingleton
    {
        private static Dictionary<string, IRepository> repos = new Dictionary<string, IRepository>();

        public static void Sync(Dictionary<string, IRepository> repoList)
        {
            repos = new Dictionary<string, IRepository>();

            foreach (var repo in repoList)
            {
                repos.Add(repo.Key, (IRepository)repo.Value.Clone());
            }
        }

        public static Dictionary<string, IRepository> GetRepo()
        {
            return repos;
        }

        public static void Clear()
        {
            repos.Clear();
        }
    }
}