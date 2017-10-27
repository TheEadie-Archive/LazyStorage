using System;
using System.Collections.Generic;
using System.Linq;
using LazyStorage.InMemory;
using LazyStorage.Interfaces;
using LazyStorage.Json;
using LazyStorage.Xml;
using Xunit;

namespace LazyStorage.Tests
{
    public class RepositoryWithConverterTests
    {
        private static IEnumerable<object[]> Repos => new[]
        {
            new object[] {new InMemoryRepositoryWithConverter<TestObjectNotIStorable>(new TestObjectStorageConverter())},
            new object[] {new XmlRepositoryWithConverter<TestObjectNotIStorable>("RepositoryWithConverterTests", new TestObjectStorageConverter())},
            new object[] {new JsonRepositoryWithConverter<TestObjectNotIStorable>("RepositoryWithConverterTests", new TestObjectStorageConverter())},
        };

        [Theory, MemberData(nameof(Repos))]
        public void CanAddToRepo(IRepository<TestObjectNotIStorable> repo)
        {
            var obj = new TestObjectNotIStorable
            {
                Name = "Test",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            repo.Set(obj);
            
            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }

        [Theory, MemberData(nameof(Repos))]
        public void CanUpdateRepo(IRepository<TestObjectNotIStorable> repo)
        {
            var obj = new TestObjectNotIStorable
            {
                Name = "Test",
                StartDate = new DateTime(2015, 12, 31, 13, 54, 23),
                EndDate = new DateTime(2015, 12, 31, 13, 54, 23)
            };
            repo.Set(obj);

            obj.StartDate = new DateTime(2015, 1, 10, 13, 54, 23);
            obj.EndDate = new DateTime(2015, 2, 28, 13, 54, 23);
            repo.Set(obj);

            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }

        [Theory, MemberData(nameof(Repos))]
        public void CanDeleteFromRepo(IRepository<TestObjectNotIStorable> repo)
        {
            var obj = new TestObjectNotIStorable { Name = "Test" };

            repo.Set(obj);
            repo.Delete(obj);

            Assert.False(repo.Get().Any(), "The object could not be deleted from the repository");
        }

        [Theory, MemberData(nameof(Repos))]
        public void CanGetByLinq(IRepository<TestObjectNotIStorable> repo)
        {
            var objOne = new TestObjectNotIStorable { Name = "one" };
            var objTwo = new TestObjectNotIStorable { Name = "two" };

            repo.Set(objOne);
            repo.Set(objTwo);

            var result = repo.Get(x => x.Name == "one").SingleOrDefault();

            Assert.NotNull(result);
            Assert.True(result.ContentEquals(objOne), "The object could not be retrieved from the repository");
        }
    }
}