using System.Linq;
using Xunit;

namespace LazyStorage.Tests.InMemory
{
    public class InMemoryFactoryTests
    {
        [Fact]
        public void CanGetMemoryStorage()
        {
            var dal = new StorageFactory().GetInMemoryStorage();
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
        }

        [Fact]
        public void SingletonWorks()
        {
            var dal = new StorageFactory().GetInMemoryStorage();
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject(); ;

            repo.Upsert(obj);
            dal.Save();

            var dal2 = new StorageFactory().GetInMemoryStorage();
            var repo2 = dal2.GetRepository<TestObject>();

            Assert.True(repo2.Get().Any(), "The object could not be added to the repository");
        }
    }
}
