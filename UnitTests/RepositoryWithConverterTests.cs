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

        public void Dispose()
        {
            m_CurrentStorage.CleanUp();
        }
    }
}