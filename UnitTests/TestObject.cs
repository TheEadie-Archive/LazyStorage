using System;
using System.Runtime.Serialization;

namespace LazyStorage.Tests
{
    public class TestObject : IStorable<TestObject>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private DateTime m_StartDate;
        private DateTime m_EndDate;

        public TestObject()
        {
            Name = "";
        }

        public SerializationInfo GetStorageInfo()
        {
            var info = new SerializationInfo(GetType(),new FormatterConverter());

            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
            info.AddValue("StartDate", m_StartDate);
            info.AddValue("EndDate", m_EndDate);

            return info;
        }

        public void InitialiseWithStorageInfo(SerializationInfo info)
        {
            Id = info.GetInt32("Id");
            Name = info.GetString("Name");
            m_StartDate = info.GetDateTime("StartDate");
            m_EndDate = info.GetDateTime("EndDate");
        }

        public bool Equals(TestObject other)
        {
            return (other.Id == Id);
        }

        public bool ContentEquals(TestObject other)
        {
            return (other.Id == Id)
                && (other.Name == Name)
                && (other.m_StartDate == m_StartDate)
                && (other.m_EndDate == m_EndDate);
        }
    }
}