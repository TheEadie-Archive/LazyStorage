using System;

namespace LazyStorage
{
    public interface IConverter<T> where T : IEquatable<T>
    {
        StorableObject GetStorableObject(T item);
        T GetOriginalObject(StorableObject info);
        bool IsEqual(StorableObject storageObject, T realObject);
    }
}