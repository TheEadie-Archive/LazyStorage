namespace LazyStorage.Xml
{
    public class XmlRepository<T> : IRepository
    {
        private readonly string m_StorageFolder;

        public XmlRepository(string storageFolder)
        {
            m_StorageFolder = storageFolder;
        }
    }
}