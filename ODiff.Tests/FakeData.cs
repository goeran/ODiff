using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ODiff.Tests.Fakes;

namespace ODiff.Tests
{
    public class FakeData
    {
        public static IEnumerable<Person> Persons
        {
            get
            {
                var persons = new List<Person>();
                var steve = new Person
                {
                    NameProperty = "Steve Jobs",
                    AgeProperty = 55,
                    NameField = "Steve Jobs",
                    AgeField = 55,
                    Children = new List<Person>
                    {
                        new Person
                        {
                            NameProperty = "Unclebob Martin",
                            AgeProperty = 30,
                            NameField = "Unclebob Martin",
                            AgeField = 30
                        }
                    }
                };
                persons.Add(steve);
                return persons;
            }
        }
    }
}
