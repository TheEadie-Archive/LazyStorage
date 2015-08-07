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
        public void CanAdd()
        {
            var repo = new XmlRepository<TestObject>(new XDocument(new XElement("Root")));
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.True(repo.Get().Any(), "The object could not be added to the repository");
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