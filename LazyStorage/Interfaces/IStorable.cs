using System;
using System.Collections.Generic;

namespace LazyStorage.Interfaces
{
    public interface IStorable<T> : IEquatable<T> where T : new()
    {
        int Id { get; set; }
        Dictionary<string, string> GetStorageInfo();
        void InitialiseWithStorageInfo(Dictionary<string, string> info);
    }
}