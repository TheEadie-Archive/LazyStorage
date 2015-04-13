using System;
using System.Linq;
using LazyLibrary.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyLibrary.Tests.Storage
{
    [TestClass]
    public class StorageFactoryTests
    {
        [TestMethod]
        public void CanGetMemoryStorage()
        {
            var dal = new StorageFactory().GetStorage();
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.IsTrue(repo.Get().Any(), "The object could not be added to the repository");
        }

        [TestMethod]
        public void SingletonWorks()
        {
            var dal = new StorageFactory().GetStorage();
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject(); ;

            repo.Upsert(obj);
            dal.Save();

            var dal2 = new StorageFactory().GetStorage();
            var repo2 = dal2.GetRepository<TestObject>();

            Assert.IsTrue(repo2.Get().Any(), "The object could not be added to the repository");
        }
    }
}
