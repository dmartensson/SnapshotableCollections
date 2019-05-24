#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace SnapshotableCollections
{
    public class Demo
    {
        public string Test(string key, string defaultValue)
        {
            var (_, value) = Test(key);
            return value?.Value ?? defaultValue;
        }

        public (MyValue? a, MyValue? f) Test(string key)
        {
            return (null, null);
        }
    }

    public class MyValue
    {
        public string Value = "Hello";
    }
}
