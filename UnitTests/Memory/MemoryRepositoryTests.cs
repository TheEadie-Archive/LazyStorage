using System.Linq;
using LazyStorage.Memory;
using Xunit;

namespace LazyStorage.Tests.Memory
{
    public class MemoryRepositoryTests
    {
        [Fact]
        public void CanAdd()
        {
            var repo = new MemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
        }

        [Fact]
        public void CanUpdate()
        {
            var repo = new MemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            obj.Name = "Test";
            repo.Upsert(obj);

            Assert.True(repo.Get().Any(), "The object could not be updated in the repository");
        }

        [Fact]
        public void CanDelete()
        {
            var repo = new MemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);
            repo.Delete(obj);

            Assert.False(repo.Get().Any(), "The object could not be deleted from the repository");
        }

        [Fact]
        public void CanGetById()
        {
            var repo = new MemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.NotNull(repo.GetById(1));
        }

        [Fact]
        public void CanGetByLinq()
        {
            var repo = new MemoryRepository<TestObject>();
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