using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ODiff.Interceptors;
using ODiff.Tests.Fakes;
using ODiff.Tests.Utils;

namespace ODiff.Tests
{
    public class Diff_complex_object_graphs
    {
        [TestFixture]
        public class When_diff_uneqal_objects
        {
            [Test]
            public void It_will_report_diff_on_public_String_properties()
            {
                var a = new Person { NameProperty = "Gøran" };
                var b = new Person { NameProperty = "Gøran Hansen" };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_String_fields()
            {
                var a = new Person { NameField = "Steve" };
                var b = new Person { NameField = "Bill" };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_decimal_properties()
            {
                var a = new Person { HeightProperty = 1.8m };
                var b = new Person { HeightProperty = 2.10m };

                var diff = Diff.ObjectValues(a, b);
                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("HeightProperty", diff.Table[0].MemberPath);
            }

            [Test]
            public void It_will_report_diff_on_public_decimal_fields()
            {
                var a = new Person { HeightField = 1.8m };
                var b = new Person { HeightField = 2.10m };

                var diff = Diff.ObjectValues(a, b);
                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("HeightField", diff.Table[0].MemberPath);
            }

            [Test]
            public void It_will_report_diff_on_public_int_fields()
            {
                var a = new Person { AgeField = 20 };
                var b = new Person { AgeField = 29 };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_int_properties()
            {
                var left = new Person { AgeProperty = 20 };
                var right = new Person { AgeProperty = 29 };

                Assert.IsTrue(Diff.ObjectValues(left, right).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_float_properties()
            {
                var left = new Person { WeightProperty = 80 };
                var right = new Person { WeightProperty = 81 };

                Assert.IsTrue(Diff.ObjectValues(left, right).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_float_field()
            {
                var left = new Person { WeightField = 70 };
                var right = new Person { WeightField = 81 };

                Assert.IsTrue(Diff.ObjectValues(left, right).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_Enum_properties()
            {
                var left = new Person { GenderProperty = Gender.Female };
                var right = new Person { GenderProperty = Gender.Male };

                var report = Diff.ObjectValues(left, right);
                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("GenderProperty", report.Table[0].MemberPath);
            }

            [Test]
            public void It_will_report_diff_on_public_Enum_fields()
            {
                var left = new Person { GenderField = Gender.Female };
                var right = new Person { GenderField = Gender.Male };

                var report = Diff.ObjectValues(left, right);
                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("GenderField", report.Table[0].MemberPath);

            }

            [Test]
            public void It_will_report_diff_on_public_DateTime_properties()
            {
                var left = new Person { BornProperty = new DateTime(1981, 10, 10) };
                var right = new Person { BornProperty = new DateTime(1981, 10, 11) };

                var diff = Diff.ObjectValues(left, right);
                Assert.AreEqual(1, diff.Table.Rows.Count());
            }

            [Test]
            public void It_will_report_diff_on_public_DateTime_fields()
            {
                var left = new Person { BornField = new DateTime(1981, 10, 10) };
                var right = new Person { BornField = new DateTime(1982, 10, 10) };

                var diff = Diff.ObjectValues(left, right);
                Assert.AreEqual(1, diff.Table.Rows.Count());
            }

            [Test]
            public void It_will_not_report_diff_on_object_references()
            {
                var a = new Person { Children = new List<Person>() };
                var b = new Person { Children = new List<Person>() };

                Assert.IsFalse(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_object_references_when_left_is_null()
            {
                var a = new Person();
                var b = new Person { Children = new List<Person>() };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_object_references_when_right_is_null()
            {
                var a = new Person { Children = new List<Person>() };
                var b = new Person();

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_multiple_diffs()
            {
                var left = new Person
                {
                    NameProperty = "Gøran",
                    AgeProperty = 29,
                    NameField = "Hansen",
                    AgeField = 31
                };
                var right = new Person
                {
                    NameProperty = "Gøran Hansen",
                    AgeProperty = 30,
                    NameField = "Mr Hansen",
                    AgeField = 32
                };

                var report = Diff.ObjectValues(left, right);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(4, report.Table.Rows.Count());
                Assert.AreEqual("NameField", report.Table[0].MemberPath);
                Assert.AreEqual("AgeField", report.Table[1].MemberPath);
                Assert.AreEqual("NameProperty", report.Table[2].MemberPath);
                Assert.AreEqual("AgeProperty", report.Table[3].MemberPath);
            }
        }

        [TestFixture]
        public class When_diff_unequal_child_objects
        {
            [Test]
            public void It_will_report_diff_on_object_properties()
            {
                var left = new Person
                {
                    Children = new List<Person>
                    {
                        new Person { NameProperty = "Steve" }
                    }
                };
                var right = new Person
                {
                    Children = new List<Person>
                    {
                        new Person { NameProperty = "Bill" }                  
                    }
                };

                var diff = Diff.ObjectValues(left, right);

                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("Children[0].NameProperty", diff.Table[0].MemberPath);
            }
        }

        [TestFixture]
        public class When_diff_graphs_with_cyclic_dependencies
        {
            [Test]
            [ExpectedException(typeof(Exception), 
                ExpectedMessage = "It's not possible to diff graphs with cyclic dependencies. Found in: Children[0]")]
            public void It_will_throw_exception_to_say_its_currently_not_possible()
            {
                var left = new Person { Children = new List<Person>() };
                left.Children.Add(left);
                var right = new Person { Children = new List<Person>() };
                right.Children.Add(right);

                Diff.ObjectValues(left, right);
            }
 
        }

        [TestFixture]
        public class When_diff_equal_objects
        {
            [Test]
            public void It_will_not_report_diff()
            {
                var steve = FakeData.KnownPersons.SteveJobs;
                var steveExactCopy = ObjectCloner.Clone(steve);

                var report = Diff.ObjectValues(steve, steveExactCopy);
                Assert.IsFalse(report.DiffFound);
            }
        }

        [TestFixture]
        public class When_diff_object_of_different_types
        {
            [Test]
            public void It_will_rapport_diff()
            {
                var left = new Buss {Brand = "Mercedes Benz", Capacity = 22};
                var right = new Car {Brand = "Mercedes Benz"};

                var report = Diff.ObjectValues(left, right);
                Console.WriteLine(report.Print());

                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("Capacity", report.Table.Rows.ElementAt(0).MemberPath);
            }

            class Car
            {
                public string Brand { get; set; } 
            }

            class Buss
            {
                public string Brand { get; set; }
                public int Capacity { get; set; }
            }
        }

    }
}
