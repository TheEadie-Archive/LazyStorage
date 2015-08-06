using System.Collections.Generic;

namespace LazyStorage.Storage.Memory
{
    internal static class MemorySingleton
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