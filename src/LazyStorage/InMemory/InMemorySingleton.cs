using System.Collections.Generic;

namespace LazyStorage.InMemory
{
    internal static class InMemorySingleton
    {
        private static readonly Dictionary<string, IEnumerable<Dictionary<string,string>>> Repos = new Dictionary<string, IEnumerable<Dictionary<string,string>>>();

        public static void Sync(string type, IEnumerable<Dictionary<string, string>> items)
        {
            if (!Repos.ContainsKey(type))
            {
                Repos.Add(type, items);
            }
            else
            {
                Repos[type] = items;
            }
        }

        public static IEnumerable<Dictionary<string, string>> GetRepo<T>()
        {
            if (Repos.ContainsKey(nameof(T)))
            {
                return Repos[nameof(T)];
            }
            else
            {
                return new List<Dictionary<string,string>>();
            }
        }

        public static void Clear()
        {
            Repos.Clear();
        }
    }
}