using System;
using System.Collections.Generic;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests
{
    public class TestObject : IStorable<TestObject>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private DateTime _startDate;
        private DateTime _endDate;

        public TestObject()
        {
            Name = "";
        }

        public Dictionary<string, string> GetStorageInfo()
        {
            var info = new Dictionary<string, string>
            {
                {"Id", Id.ToString()},
                {"Name", Name},
                {"StartDate", _startDate.Ticks.ToString()},
                {"EndDate", _endDate.Ticks.ToString()}
            };

            return info;
        }

        public void InitialiseWithStorageInfo(Dictionary<string, string> info)
        {
            Id = int.Parse(info["Id"]);
            Name = info["Name"];
            _startDate = new DateTime(long.Parse(info["StartDate"]));
            _endDate = new DateTime(long.Parse(info["EndDate"]));
        }

        public bool Equals(TestObject other)
        {
            return other.Id == Id;
        }

        public bool ContentEquals(TestObject other)
        {
            return other.Id == Id
                && other.Name == Name
                && other._startDate == _startDate
                && other._endDate == _endDate;
        }
    }
}