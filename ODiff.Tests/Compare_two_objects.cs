using System.Collections.Generic;
using NUnit.Framework;

namespace ODiff.Tests
{
    public class Compare_two_objects
    {
        [TestFixture]
        public class When_diff_uneqal_objects
        {
            [Test]
            public void It_will_report_diff_on_public_String_properties()
            {
                var a = new Person {NameProperty = "Gøran"};
                var b = new Person {NameProperty = "Gøran Hansen"};

                Assert.IsTrue(Diff.Object(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_String_fields()
            {
                var a = new Person {NameAsField = "Steve"};
                var b = new Person {NameAsField = "Bill"};

                Assert.IsTrue(Diff.Object(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_int_properties()
            {
                var a = new Person {AgeAsProperty = 20};
                var b = new Person {AgeAsProperty = 29};

                Assert.IsTrue(Diff.Object(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_int_fields()
            {
                var a = new Person { AgeAsField = 20 };
                var b = new Person { AgeAsField = 29 };

                Assert.IsTrue(Diff.Object(a, b).DiffFound);
            }

            [Test]
            public void It_will_not_report_diff_on_object_references()
            {
                var a = new Person {Assets = new List<object>()};
                var b = new Person {Assets = new List<object>()};

                Assert.IsFalse(Diff.Object(a, b).DiffFound);
            }
        }

        [TestFixture]
        public class When_diff_object_properties_with_primitive_values
        {
            [Test]
            public void It_will_not_report_diff_if_values_are_equal()
            {
                var a = new Person();
                a.NameProperty = "Gøran";
                var b = new Person();
                b.NameProperty = "Gøran";

                DiffResult result = Diff.Object(a, b);
                
                Assert.AreEqual(false, result.DiffFound);
            }
        }  
      
        private class Person
        {
            public string NameProperty { get; set; }
            public int AgeAsProperty { get; set; }
            public List<object> Assets { get; set; }

            public string NameAsField;
            public int AgeAsField;
        }
    }
}
