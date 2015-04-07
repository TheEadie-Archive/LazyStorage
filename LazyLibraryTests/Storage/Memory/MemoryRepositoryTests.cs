using LazyLibrary.Storage;
using LazyLibrary.Storage.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LazyLibrary.Tests.Storage.Memory
{
    [TestClass]
    public class MemoryRepositoryTests
    {
        [TestMethod]
        public void CanAdd()
        {
            var repo = new MemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.IsTrue(repo.Get().Any(), "The object could not be added to the repository");
        }

        [TestMethod]
        public void CanGetById()
        {
            var repo = new MemoryRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.IsNotNull(repo.GetById(1), "The object could not be retrieved from the repository");
        }
    }
}