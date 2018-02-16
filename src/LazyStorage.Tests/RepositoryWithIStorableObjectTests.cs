using System.Collections.Generic;
using System.Linq;
using LazyStorage.InMemory;
using LazyStorage.Interfaces;
using LazyStorage.Json;
using LazyStorage.Xml;
using Xunit;

namespace LazyStorage.Tests
{
    public class RepositoryWithIStorableObjectTests
    {
        public static IEnumerable<object[]> Repos => new[]
        {
            new object[] {new InMemoryRepositoryWithConverter<TestObjectIStorable>(new StorableConverter<TestObjectIStorable>())},
            new object[] {new XmlRepositoryWithConverter<TestObjectIStorable>("RepositoryWithIStorableObjectTests", new StorableConverter<TestObjectIStorable>()) },
            new object[] {new JsonRepositoryWithConverter<TestObjectIStorable>("RepositoryWithIStorableObjectTests", new StorableConverter<TestObjectIStorable>()) }
        };

        [Theory, MemberData(nameof(Repos))]
        public void CanAddToRepo(IRepository<TestObjectIStorable> repo)
        {
            var obj = new TestObjectIStorable();

            repo.Set(obj);
            
            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }
        
        [Theory, MemberData(nameof(Repos))]
        public void CanUpdateRepo(IRepository<TestObjectIStorable> repo)
        {
            var obj = new TestObjectIStorable();

            repo.Set(obj);

            obj.Name = "Test";
            repo.Set(obj);

            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }

        [Theory, MemberData(nameof(Repos))]
        public void CanDelete(IRepository<TestObjectIStorable> repo)
        {
            var obj = new TestObjectIStorable();

            repo.Set(obj);
            repo.Delete(obj);

            Assert.False(repo.Get().Any(), "The object could not be deleted from the repository");
        }

        [Theory, MemberData(nameof(Repos))]
        public void CanGetByLinq(IRepository<TestObjectIStorable> repo)
        {
            var objOne = new TestObjectIStorable {Id = 1, Name = "one"};
            var objTwo = new TestObjectIStorable {Id = 2, Name = "two"};

            repo.Set(objOne);
            repo.Set(objTwo);

            var result = repo.Get(x => x.Name == "one").SingleOrDefault();

            Assert.NotNull(result);
            Assert.True(result.Equals(objOne), "The object could not be retrieved from the repository");
        }
    }
}