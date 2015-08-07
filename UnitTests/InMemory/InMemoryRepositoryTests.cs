using System.Linq;
using LazyStorage.InMemory;
using Xunit;

namespace LazyStorage.Tests.InMemory
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public void CanAdd()
        {
            var repo = new InMemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
        }

        [Fact]
        public void CanUpdate()
        {
            var repo = new InMemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            obj.Name = "Test";
            repo.Upsert(obj);

            Assert.True(repo.Get().Any(), "The object could not be updated in the repository");
        }

        [Fact]
        public void CanDelete()
        {
            var repo = new InMemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);
            repo.Delete(obj);

            Assert.False(repo.Get().Any(), "The object could not be deleted from the repository");
        }

        [Fact]
        public void CanGetById()
        {
            var repo = new InMemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.NotNull(repo.GetById(1));
        }

        [Fact]
        public void CanGetByLinq()
        {
            var repo = new InMemoryRepository<TestObject>();
            var objOne = new TestObject {Name = "one"};
            var objTwo = new TestObject {Name = "two"};

            repo.Upsert(objOne);
            repo.Upsert(objTwo);

            var result = repo.Get(x => x.Name == "one").SingleOrDefault();

            Assert.NotNull(result);
            Assert.True(result.Equals(objOne), "The object could not be retrieved from the repository");
        }
    }
}