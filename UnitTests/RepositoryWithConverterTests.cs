using System;
using System.Collections.Generic;
using System.Linq;
using LazyStorage.Tests.StorageTypes;
using Xunit;

namespace LazyStorage.Tests
{
    public class RepositoryWithConverterTests
    {
        public static IEnumerable<object[]> StorageTypes => new[]
        {
            new object[] {new InMemoryTestStorage(), },
            new object[] {new XmlTestStorage(), },
        };

        private ITestStorage m_CurrentStorage;

        [Theory, MemberData("StorageTypes")]
        public void CanAddToRepo(ITestStorage storage)
        {
            m_CurrentStorage = storage;

            var converter = new TestObjectStorageConverter();

            var repo = storage.GetStorage().GetRepository(converter);

            var obj = new TestObjectNotIStorable();
            obj.Name = "Test";
            obj.StartDate = DateTime.Now;
            obj.EndDate = DateTime.Now;

            repo.Upsert(obj);
            
            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }

        [Theory, MemberData("StorageTypes")]
        public void CanUpdateRepo(ITestStorage storage)
        {
            m_CurrentStorage = storage;

            var converter = new TestObjectStorageConverter();

            var repo = storage.GetStorage().GetRepository(converter);

            var obj = new TestObjectNotIStorable();
            obj.Name = "Test";
            obj.StartDate = DateTime.Now;
            obj.EndDate = DateTime.Now;
            repo.Upsert(obj);

            obj.StartDate = DateTime.Now;
            obj.EndDate = DateTime.Now;
            repo.Upsert(obj);

            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }

        [Theory, MemberData("StorageTypes")]
        public void CanDeleteFromRepo(ITestStorage storage)
        {
            m_CurrentStorage = storage;
            var converter = new TestObjectStorageConverter();

            var repo = storage.GetStorage().GetRepository(converter);

            var obj = new TestObjectNotIStorable();
            obj.Name = "Test";

            repo.Upsert(obj);
            repo.Delete(obj);

            Assert.False(repo.Get().Any(), "The object could not be deleted from the repository");
        }

        [Theory, MemberData("StorageTypes")]
        public void CanGetByLinq(ITestStorage storage)
        {
            m_CurrentStorage = storage;
            var converter = new TestObjectStorageConverter();

            var repo = storage.GetStorage().GetRepository(converter);
            var objOne = new TestObjectNotIStorable { Name = "one" };
            var objTwo = new TestObjectNotIStorable { Name = "two" };

            repo.Upsert(objOne);
            repo.Upsert(objTwo);

            var result = repo.Get(x => x.Name == "one").SingleOrDefault();

            Assert.NotNull(result);
            Assert.True(result.Equals(objOne), "The object could not be retrieved from the repository");
        }

        public void Dispose()
        {
            m_CurrentStorage.CleanUp();
        }
    }
}