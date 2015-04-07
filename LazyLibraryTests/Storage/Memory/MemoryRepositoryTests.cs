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
    }

    public class TestObject : IStorable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }
}