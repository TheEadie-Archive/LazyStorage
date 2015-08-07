using System.Linq;
using System.Xml.Linq;
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
    }
}