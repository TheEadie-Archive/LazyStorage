using System;
using System.IO;
using System.Linq;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests.StorageTypes
{
    public class JsonTestStorage : ITestStorage
    {
        private readonly IStorage m_Storage = StorageFactory.GetJsonStorage(@"");

        public IStorage GetStorage()
        {
            return m_Storage;
        }

        public void CleanUp()
        {
            var di = new DirectoryInfo(Environment.CurrentDirectory);
            var files = di.GetFiles("*.json").Where(x => x.Extension == ".json");

            foreach (var file in files)
            {
                File.Delete(file.FullName);
            }
        }
    }
}