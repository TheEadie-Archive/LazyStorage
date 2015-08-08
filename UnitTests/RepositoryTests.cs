using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LazyStorage.InMemory;
using LazyStorage.Xml;
using Xunit;

namespace LazyStorage.Tests
{
    public class RepositoryTests
    {
        public static IEnumerable<object[]> Repos
        {
            get
            {
                return new[]
                {
                    new object[] {new InMemoryRepository<TestObject>()},
                    new object[] {new XmlRepository<TestObject>(new XDocument(new XElement("Root")))}
                };
            }
        }

        [Theory, MemberData("Repos")]
        public void CanAddToRepo(IRepository<TestObject> repo)
        {
            var obj = new TestObject();

            repo.Upsert(obj);
            
            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }
        
        [Theory, MemberData("Repos")]
        public void CanUpdateRepo(IRepository<TestObject> repo)
        {
            var obj = new TestObject();

            repo.Upsert(obj);

            obj.Name = "Test";
            repo.Upsert(obj);

            var repoObj = repo.Get().Single();

            Assert.True(repoObj.ContentEquals(obj), "The object returned does not match the one added");
        }

        [Theory, MemberData("Repos")]
        public void CanDelete(IRepository<TestObject> repo)
        {
            var obj = new TestObject();

            repo.Upsert(obj);
            repo.Delete(obj);

            Assert.False(repo.Get().Any(), "The object could not be deleted from the repository");
        }

        [Theory, MemberData("Repos")]
        public void CanGetById(IRepository<TestObject> repo)
        {
            var obj = new TestObject();

            repo.Upsert(obj);

            Assert.NotNull(repo.GetById(1));
        }

        [Theory, MemberData("Repos")]
        public void CanGetByLinq(IRepository<TestObject> repo)
        {
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