using System.IO;

namespace LazyStorage.Tests.StorageTypes
{
    public class XmlTestStorage : ITestStorage
    {
        private readonly IStorage m_storage = new StorageFactory().GetXmlStorage(@"");

        public IStorage GetStorage()
        {
            return m_storage;
        }

        public void CleanUp()
        {
            File.Delete("LazyStorage.Xml");
        }
    }
}