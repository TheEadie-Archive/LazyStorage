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

        [TestMethod]
        public void CanGetByLINQ()
        {
            var repo = new MemoryRepository<TestObject>();
            var objOne = new TestObject() { Name = "one" };
            var objTwo = new TestObject() { Name = "two" };

            repo.Upsert(objOne);
            repo.Upsert(objTwo);

            var result = repo.Get(x => x.Name == "one").SingleOrDefault();

            Assert.IsNotNull(result, "The object could not be retrieved from the repository");
            Assert.IsTrue(result.Equals(objOne), "The object could not be retrieved from the repository");
        }
    }
}