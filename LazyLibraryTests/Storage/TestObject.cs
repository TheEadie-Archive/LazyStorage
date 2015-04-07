using LazyLibrary.Storage;
using System;

namespace LazyLibrary.Tests.Storage
{
    public class TestObject : IStorable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }
}