using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace LazyLibrary.Storage.Memory
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
