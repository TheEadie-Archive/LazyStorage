using System.Collections.Generic;

namespace LazyStorage.InMemory
{
    internal static class InMemorySingleton
    {
        private static Dictionary<string, IRepository> repos = new Dictionary<string, IRepository>();

        public static void Sync(Dictionary<string, IRepository> repoList)
        {
            repos = repoList;
        }

        public static Dictionary<string, IRepository> GetRepo()
        {
            return repos;
        }
    }
}