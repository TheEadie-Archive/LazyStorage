using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LazyLibrary.Storage;
using LazyLibrary.Storage.Memory;

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

            Assert.IsTrue(repo.Get().Any());
        }
    }

    public class TestObject : IStorable
    {
        public int Id { get; set; }
    }
}
