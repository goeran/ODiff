using System;
using System.Collections.Generic;
using System.Linq;

namespace ODiff.Tests.Fakes
{
    [Serializable]
    public class Person
    {
        public int Id { get; set; }
        public string NameProperty { get; set; }
        public int AgeProperty { get; set; }
        public List<Person> Children { get; set; }
        public Gender GenderProperty { get; set; }
        public decimal HeightProperty { get; set; }
        public float WeightProperty { get; set; }
        public DateTime BornProperty { get; set; }

        public string NameField;
        public int AgeField;
        public List<string> Tags;

        private readonly List<string> phones = new List<string>();
        public IEnumerable<string> Phones { get { return phones.OrderBy(p => p); } }
        public void AddPhone(string phone)
        {
            phones.Add(phone);
        }

        public Gender GenderField;
        public decimal HeightField;
        public float WeightField;
        public DateTime BornField;

    }
}
