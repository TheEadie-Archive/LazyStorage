using System;

namespace LazyStorage.Tests
{
    public class TestObject : IStorable<TestObject>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Equals(TestObject other)
        {
            return (other.Id == Id);
        }
    }
}