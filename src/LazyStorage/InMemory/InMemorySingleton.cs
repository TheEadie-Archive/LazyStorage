using System.Collections.Generic;

namespace LazyStorage.InMemory
{
    internal static class InMemorySingleton
    {
        private static Dictionary<string, IEnumerable<Dictionary<string,string>>> _repos = new Dictionary<string, IEnumerable<Dictionary<string,string>>>();

        public static void Sync(string type, IEnumerable<Dictionary<string, string>> items)
        {
            if (!_repos.ContainsKey(type))
            {
                _repos.Add(type, items);
            }
            else
            {
                _repos[type] = items;
            }
        }

        public static IEnumerable<Dictionary<string, string>> GetRepo<T>()
        {
            if (_repos.ContainsKey(nameof(T)))
            {
                return _repos[nameof(T)];
            }
            else
            {
                return new List<Dictionary<string,string>>();
            }
        }

        public static void Clear()
        {
            _repos.Clear();
        }
    }
}