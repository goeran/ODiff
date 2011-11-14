using System;
using System.Collections.Generic;

namespace ODiff.Tests.Fakes
{
    [Serializable]
    public class Person
    {
        public string NameProperty { get; set; }
        public int AgeProperty { get; set; }
        public List<Person> Children { get; set; }
        public Gender GenderProperty { get; set; }

        public string NameField;
        public int AgeField;
        public List<string> Tags;

    }
}
