using System;
using System.IO;
using System.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests.StorageTypes
{
    public class XmlTestStorage : ITestStorage
    {
        private readonly IStorage _storage = StorageFactory.GetXmlStorage(@"");

        public IStorage GetStorage()
        {
            return _storage;
        }

        public void CleanUp()
        {
            var di = new DirectoryInfo(Environment.CurrentDirectory);
            var files = di.GetFiles("*.xml").Where(x => x.Extension == ".xml");

            foreach (var file in files)
            {
                File.Delete(file.FullName);
            }
        }
    }
}