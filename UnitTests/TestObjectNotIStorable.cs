using System;
using System.Runtime.Serialization;
using LazyStorage.Interfaces;

namespace LazyStorage.Tests
{
    public class TestObjectNotIStorable
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TestObjectNotIStorable()
        {
            Name = "";
        }

        public bool ContentEquals(TestObjectNotIStorable other)
        {
            return (other.Name == Name)
                && (other.StartDate == StartDate)
                && (other.EndDate == EndDate);
        }
    }

    public class TestObjectStorageConverter : IConverter<TestObjectNotIStorable>
    {
        public StorableObject GetStorableObject(TestObjectNotIStorable item)
        {
            var storableObject = new StorableObject();

            storableObject.Info.AddValue("Name", item.Name);
            storableObject.Info.AddValue("StartDate", item.StartDate);
            storableObject.Info.AddValue("EndDate", item.EndDate);

            return storableObject;
        }

        public TestObjectNotIStorable GetOriginalObject(StorableObject info)
        {
            var orginalObject = new TestObjectNotIStorable();

            orginalObject.Name = info.Info.GetString("Name");
            orginalObject.StartDate = info.Info.GetDateTime("StartDate");
            orginalObject.EndDate = info.Info.GetDateTime("EndDate");

            return orginalObject;
        }

        public bool IsEqual(StorableObject storageObject, TestObjectNotIStorable realObject)
        {
            return realObject.Name == storageObject.Info.GetString("Name");
        }
    }
}