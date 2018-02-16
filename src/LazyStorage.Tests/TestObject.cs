using System;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests
{
    public class TestObject
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TestObject()
        {
            Name = "";
        }

        public bool ContentEquals(TestObject other)
        {
            return (other.Name == Name)
                && (other.StartDate == StartDate)
                && (other.EndDate == EndDate);
        }
    }

    public class TestObjectStorageConverter : IConverter<TestObject>
    {
        public StorableObject GetStorableObject(TestObject item)
        {
            var storableObject = new StorableObject();

            storableObject.Info.Add("Id", item.Name);
            storableObject.Info.Add("StartDate", item.StartDate.Ticks.ToString());
            storableObject.Info.Add("EndDate", item.EndDate.Ticks.ToString());

            return storableObject;
        }

        public TestObject GetOriginalObject(StorableObject info)
        {
            var orginalObject = new TestObject
            {
                Name = info.Info["Id"],
                StartDate = new DateTime(long.Parse(info.Info["StartDate"])),
                EndDate = new DateTime(long.Parse(info.Info["EndDate"]))
            };

            return orginalObject;
        }

        public bool IsEqual(StorableObject storageObject, TestObject realObject)
        {
            return realObject.Name == storageObject.Info["Id"];
        }
    }
}