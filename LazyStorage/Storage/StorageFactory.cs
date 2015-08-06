﻿using LazyLibrary.Storage.Memory;

namespace LazyLibrary.Storage
{
    public class StorageFactory
    {
        private IStorage m_Store;

        public IStorage GetStorage()
        {
            if (m_Store == null)
            {
                m_Store = new MemoryStorage();
            }

            return m_Store;
        }
    }
}