using System;
using System.Collections.Generic;
using System.Linq;
using LazyStorage.Tests.StorageTypes;
using Xunit;

namespace LazyStorage.Tests
{
    public sealed class PersistenceTests : IDisposable
    {
        public static IEnumerable<object[]> StorageTypes => new[]
        {
            new object[] {new InMemoryTestStorage()},
            new object[] {new XmlTestStorage()},
            new object[] {new JsonTestStorage()},
        };

        private ITestStorage _currentStorage;

        [Theory, MemberData(nameof(StorageTypes))]
        public void CanSaveToStorage(ITestStorage storage)
        {
            _currentStorage = storage;
            var dal = storage.GetStorage();
            var repo = dal.GetRepository<TestObjectIStorable>();
            var obj = new TestObjectIStorable();

            repo.Set(obj);
            dal.Save();
            
            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
        }

        [Theory, MemberData(nameof(StorageTypes))]
        public void StoragePersistsBetweenSessions(ITestStorage storage)
        {
            _currentStorage = storage;
            var dal = storage.GetStorage();
            var repo = dal.GetRepository<TestObjectIStorable>();
            var obj = new TestObjectIStorable();

            repo.Set(obj);
            dal.Save();

            var dal2 = storage.GetStorage();
            var repo2 = dal2.GetRepository<TestObjectIStorable>();

            Assert.True(repo2.Get().Single().ContentEquals(obj), "The object could not be found in the persistent repo");
        }

        [Theory, MemberData(nameof(StorageTypes))]
        public void StorageDoesNotPersistIfDiscarded(ITestStorage storage)
        {
            _currentStorage = storage;

            // Create an object in memory
            var obj1 = new TestObjectIStorable();

            // Insert into the repo
            var dal = storage.GetStorage();
            var repo = dal.GetRepository<TestObjectIStorable>();
            repo.Set(obj1);
            dal.Save();

            // Make some changes
            var obj2 = new TestObjectIStorable
            {
                Id = 1,
                Name = "Test"
            };

            // Update the object in the repo but discard changes
            repo.Set(obj2);
            dal.Discard();

            var dal2 = storage.GetStorage();
            var repo2 = dal2.GetRepository<TestObjectIStorable>();

            var testObject = repo2.Get().Single();
            Assert.True(testObject.ContentEquals(obj1), "The object changes were not reverted in the repo");
        }

        [Theory, MemberData(nameof(StorageTypes))]
        public void CanSaveToStorageWithConverter(ITestStorage storage)
        {
            _currentStorage = storage;
            var dal = storage.GetStorage();
            var converter = new TestObjectStorageConverter();

            var repo = storage.GetStorage().GetRepository(converter);

            var obj = new TestObject
            {
                Name = "Test",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            repo.Set(obj);
            dal.Save();

            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
        }

        [Theory, MemberData(nameof(StorageTypes))]
        public void StoragePersistsBetweenSessionsWithConverter(ITestStorage storage)
        {
            _currentStorage = storage;
            var dal = storage.GetStorage();
            var converter = new TestObjectStorageConverter();

            var repo = dal.GetRepository(converter);

            var obj = new TestObject
            {
                Name = "Test",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            repo.Set(obj);
            dal.Save();

            var dal2 = storage.GetStorage();
            var repo2 = dal2.GetRepository(converter);

            Assert.True(repo2.Get().Single().ContentEquals(obj), "The object could not be found in the persistent repo");
        }

        [Theory, MemberData(nameof(StorageTypes))]
        public void StorageDoesNotPersistIfDiscardedWithConverter(ITestStorage storage)
        {
            _currentStorage = storage;

            // Insert into the repo
            var dal = storage.GetStorage();
            var converter = new TestObjectStorageConverter();

            var repo = dal.GetRepository(converter);

            var obj1 = new TestObject {Name = "Test"};

            repo.Set(obj1);
            dal.Save();

            // Make some changes
            var obj2 = new TestObject {Name = "Test"};

            // Update the object in the repo but discard changes
            repo.Set(obj2);
            dal.Discard();

            var dal2 = storage.GetStorage();
            var repo2 = dal2.GetRepository(converter);

            var testObject = repo2.Get().Single();
            Assert.True(testObject.ContentEquals(obj1), "The object changes were not reverted in the repo");
        }

        public void Dispose()
        {
            _currentStorage.CleanUp();
        }
    }
}
