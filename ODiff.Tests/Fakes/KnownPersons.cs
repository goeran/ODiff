using System.Collections.Generic;

namespace ODiff.Tests.Fakes
{
    public class KnownPersons
    {
        public IEnumerable<Person> All
        {
            get
            {
                var persons = new List<Person>();
                persons.Add(SteveJobs);
                return persons;
            }
        }

        public Person SteveJobs
        {
            get
            {
                return new Person
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
                        },
                        new Person
                        {
                            NameProperty = "Larry Ellison",
                            AgeProperty = 32,
                            NameField = "LarryEllison",
                            AgeField = 32
                        }
                    }
                };
            }
        }
    }
}
