using System;
using System.Runtime.Serialization;

namespace LazyStorage.Tests
{
    public class TestObject : IStorable<TestObject>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private readonly DateTime m_StartDate;
        private readonly DateTime m_EndDate;

        public TestObject()
        {
        }

        public bool Equals(TestObject other)
        {
            return (other.Id == Id);
        }

        protected TestObject(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetInt32("Id");
            Name = info.GetString("Name");
            m_StartDate = info.GetDateTime("StartDate");
            m_EndDate = info.GetDateTime("EndDate");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
            info.AddValue("StartDate", m_StartDate);
            info.AddValue("EndDate", m_EndDate);
        }

        
    }
}