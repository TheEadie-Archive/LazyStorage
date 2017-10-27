using System;
using System.Collections.Generic;
using LazyStorage.Interfaces;

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

        public Dictionary<string, string> GetStorageInfo()
        {
            var info = new Dictionary<string, string>();

            info.Add("Id", Id.ToString());
            info.Add("Name", Name);
            info.Add("StartDate", m_StartDate.Ticks.ToString());
            info.Add("EndDate", m_EndDate.Ticks.ToString());

            return info;
        }

        public void InitialiseWithStorageInfo(Dictionary<string, string> info)
        {
            Id = int.Parse(info["Id"]);
            Name = info["Name"];
            m_StartDate = new DateTime(long.Parse(info["StartDate"]));
            m_EndDate = new DateTime(long.Parse(info["EndDate"]));
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