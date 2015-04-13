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

            Assert.IsTrue(repo.Get(x => x.Id > 0).Any(), "The object could not be added to the repository");
        }
    }
}
