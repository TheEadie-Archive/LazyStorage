using System;
using LazyStorage.Interfaces;

namespace LazyStorage.Xml
{
    internal class StorableConverter<T> : IConverter<T> where T : IStorable<T>, IEquatable<T>, new()
    {
        public StorableObject GetStorableObject(T item)
        {
            return new StorableObject(item.GetStorageInfo());
        }

        public T GetOriginalObject(StorableObject info)
        {
            var item = new T();
            item.InitialiseWithStorageInfo(info.Info);
            return item;
        }

        public bool IsEqual(StorableObject storageObject, T realObject)
        {
            return GetOriginalObject(storageObject).Equals(realObject);
        }
    }
}