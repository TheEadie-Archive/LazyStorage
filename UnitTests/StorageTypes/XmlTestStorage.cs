using System.IO;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests.StorageTypes
{
    public class XmlTestStorage : ITestStorage
    {
        private readonly IStorage m_Storage = StorageFactory.GetXmlStorage(@"");

        public IStorage GetStorage()
        {
            return m_Storage;
        }

        public void CleanUp()
        {
            File.Delete("LazyStorage.Xml");
        }
    }
}