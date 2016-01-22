using LazyStorage.Interfaces;

namespace LazyStorage.Tests.StorageTypes
{
    public interface ITestStorage
    {
        IStorage GetStorage();
        void CleanUp();
    }
}