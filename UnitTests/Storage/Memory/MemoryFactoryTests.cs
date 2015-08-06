﻿using System.Linq;
using LazyLibrary.Storage;
using Xunit;

namespace LazyLibrary.Tests.Storage.Memory
{
    public class MemoryFactoryTests
    {
        [Fact]
        public void CanGetMemoryStorage()
        {
            var dal = new StorageFactory().GetStorage();
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
        }

        [Fact]
        public void SingletonWorks()
        {
            var dal = new StorageFactory().GetStorage();
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject(); ;

            repo.Upsert(obj);
            dal.Save();

            var dal2 = new StorageFactory().GetStorage();
            var repo2 = dal2.GetRepository<TestObject>();

            Assert.True(repo2.Get().Any(), "The object could not be added to the repository");
        }
    }
}