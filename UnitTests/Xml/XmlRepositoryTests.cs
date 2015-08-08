using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LazyStorage.InMemory;
using LazyStorage.Xml;
using Xunit;

namespace LazyStorage.Tests.Xml
{
    public class XmlRepositoryTests
    {
        [Fact]
        public void CanAddToRepo()
        {
            var repo = new XmlRepository<TestObject>(new XDocument(new XElement("Root")));
            var obj = new TestObject();

            repo.Upsert(obj);
            
            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }

        [Fact]
        public void CanGetById()
        {
            var repo = new XmlRepository<TestObject>(new XDocument(new XElement("Root")));
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.NotNull(repo.GetById(1));
        }

        [Fact]
        public void CanGetByLinq()
        {
            var repo = new XmlRepository<TestObject>(new XDocument(new XElement("Root")));
            var objOne = new TestObject {Name = "one"};
            var objTwo = new TestObject {Name = "two"};

            repo.Upsert(objOne);
            repo.Upsert(objTwo);

            var result = repo.Get(x => x.Name == "one").SingleOrDefault();

            Assert.NotNull(result);
            Assert.True(result.Equals(objOne), "The object could not be retrieved from the repository");
        }
    }
}