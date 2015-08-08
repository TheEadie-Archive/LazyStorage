using System;
using System.Collections.Generic;
using System.Linq;
using LazyStorage.Tests.StorageTypes;
using Xunit;

namespace LazyStorage.Tests
{
    public class PersistenceTests : IDisposable
    {
        public static IEnumerable<object[]> StorageTypes
        {
            get
            {
                return new[]
                {
                    new object[] {new InMemoryTestStorage(), },
                    new object[] {new XmlTestStorage()},
                };
            }
        }

        private ITestStorage currentStorage;

        [Theory, MemberData("StorageTypes")]
        public void CanSaveToStorage(ITestStorage storage)
        {
            currentStorage = storage;
            var dal = storage.GetStorage();
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject(); ;

            repo.Upsert(obj);
            dal.Save();
            
            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
        }

        [Theory, MemberData("StorageTypes")]
        public void StoragePersistsBetweenSessions(ITestStorage storage)
        {
            currentStorage = storage;
            var dal = storage.GetStorage();
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject(); ;

            repo.Upsert(obj);
            dal.Save();

            var dal2 = storage.GetStorage();
            var repo2 = dal2.GetRepository<TestObject>();

            Assert.True(repo2.Get().Single().ContentEquals(obj), "The object could not be found in the persistent repo");
        }

        [Theory, MemberData("StorageTypes")]
        public void StorageDoesNotPersistIfDiscarded(ITestStorage storage)
        {
            currentStorage = storage;

            // Create an object in memory
            var obj1 = new TestObject();

            // Insert into the repo
            var dal = storage.GetStorage();
            var repo = dal.GetRepository<TestObject>();
            repo.Upsert(obj1);
            dal.Save();

            // Make some changes
            var obj2 = new TestObject();
            obj2.Id = 1;
            obj2.Name = "Test";

            // Update the object in the repo but discard changes
            repo.Upsert(obj2);
            dal.Discard();

            Assert.True(repo.Get().Single().ContentEquals(obj1), "The object changes were not reverted in the repo");
        }

        public void Dispose()
        {
            currentStorage.CleanUp();
        }
    }
}
