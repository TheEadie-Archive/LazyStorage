using System;
using System.IO;
using System.Linq;
using Xunit;

namespace LazyStorage.Tests.Xml
{
    public class XmlFactoryTests : IDisposable
    {
        [Fact]
        public void CanSaveToXmlStorage()
        {
            var dal = new StorageFactory().GetXmlStorage(@"");
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject(); ;

            repo.Upsert(obj);
            dal.Save();
            
            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
        }

        [Fact]
        public void XmlStoragePersists()
        {
            var dal = new StorageFactory().GetXmlStorage(@"");
            var repo = dal.GetRepository<TestObject>();
            var obj = new TestObject(); ;

            repo.Upsert(obj);
            dal.Save();

            var dal2 = new StorageFactory().GetXmlStorage(@"");
            var repo2 = dal2.GetRepository<TestObject>();

            Assert.True(repo2.Get().Single().ContentEquals(obj), "The object could not be found in the persistent repo");
        }

        public void Dispose()
        {
            File.Delete("LazyStorage.Xml");
        }
    }
}
