using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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
                Assert.IsTrue(Diff.ObjectValues(b, a).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_String_fields()
            {
                var a = new Person { NameField = "Steve" };
                var b = new Person { NameField = "Bill" };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
                Assert.IsTrue(Diff.ObjectValues(b, a).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_decimal_properties()
            {
                var personA = new Person {HeightProperty = 1.8m};
                var personB = new Person {HeightProperty = 2.10m};

                Cross.diff(personA, personB, (left, right) =>
                {
                    var diff = Diff.ObjectValues(left, right);
                    Assert.AreEqual(1, diff.Table.Rows.Count());
                    Assert.AreEqual("HeightProperty", diff.Table[0].MemberPath);
                });
            }

            [Test]
            public void It_will_report_diff_on_public_decimal_fields()
            {
                var personA = new Person {HeightField = 1.8m};
                var personB = new Person {HeightField = 2.10m};
                Cross.diff(personA, personB, (left, right) =>
                {
                    var diff = Diff.ObjectValues(left, right);
                    Assert.AreEqual(1, diff.Table.Rows.Count());
                    Assert.AreEqual("HeightField", diff.Table[0].MemberPath);
                });
            }

            [Test]
            public void It_will_report_diff_on_public_int_fields()
            {
                var a = new Person { AgeField = 20 };
                var b = new Person { AgeField = 29 };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
                Assert.IsTrue(Diff.ObjectValues(b, a).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_int_properties()
            {
                var a = new Person { AgeProperty = 20 };
                var b = new Person { AgeProperty = 29 };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
                Assert.IsTrue(Diff.ObjectValues(b, a).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_float_properties()
            {
                var a = new Person { WeightProperty = 80 };
                var b = new Person { WeightProperty = 81 };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
                Assert.IsTrue(Diff.ObjectValues(b, a).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_float_field()
            {
                var a = new Person { WeightField = 70 };
                var b = new Person { WeightField = 81 };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
                Assert.IsTrue(Diff.ObjectValues(b, a).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_Enum_properties()
            {
                Cross.diff(
                    left: new Person { GenderProperty = Gender.Female },
                    right: new Person { GenderProperty = Gender.Male },
                    assert: (left, right) =>
                    {
                        var report = Diff.ObjectValues(left, right);
                        Assert.IsTrue(report.DiffFound);
                        Assert.AreEqual(1, report.Table.Rows.Count());
                        Assert.AreEqual("GenderProperty", report.Table[0].MemberPath);
                    }
                );
            }

            [Test]
            public void It_will_report_diff_on_public_Enum_fields()
            {
                var personA = new Person {GenderField = Gender.Female};
                var personB = new Person {GenderField = Gender.Male};
                Cross.diff(personA, personB, (left, right) =>
                {
                    var report = Diff.ObjectValues(left, right);
                    Assert.IsTrue(report.DiffFound);
                    Assert.AreEqual(1, report.Table.Rows.Count());
                    Assert.AreEqual("GenderField", report.Table[0].MemberPath);
                });
            }

            [Test]
            public void It_will_report_diff_on_public_DateTime_properties()
            {
                var personA = new Person {BornProperty = new DateTime(1981, 10, 10)};
                var personB = new Person {BornProperty = new DateTime(1981, 10, 11)};
                Cross.diff(personA, personB, (left, right) =>
                {
                    var diff = Diff.ObjectValues(left, right);
                    Assert.AreEqual(1, diff.Table.Rows.Count());
                });
            }

            [Test]
            public void It_will_report_diff_on_public_DateTime_fields()
            {
                var personA = new Person {BornField = new DateTime(1981, 10, 10)};
                var personB = new Person {BornField = new DateTime(1982, 10, 10)};
                Cross.diff(personA, personB, (left, right) =>
                {
                    var diff = Diff.ObjectValues(left, right);
                    Assert.AreEqual(1, diff.Table.Rows.Count());
                });
            }

            [Test]
            public void It_will_not_report_diff_on_object_references()
            {
                var a = new Person { Children = new List<Person>() };
                var b = new Person { Children = new List<Person>() };

                Assert.IsFalse(Diff.ObjectValues(a, b).DiffFound);
                Assert.IsFalse(Diff.ObjectValues(b, a).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_object_when_one_of_the_sides_are_null()
            {
                var a = new Person { Children = new List<Person>() };
                var b = new Person();

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
                Assert.IsTrue(Diff.ObjectValues(b, a).DiffFound);
            }

            [Test]
            public void It_will_report_multiple_diffs()
            {
                var personA = new Person
                {
                    NameProperty = "Gøran", AgeProperty = 29, NameField = "Hansen", AgeField = 31
                };
                var personB = new Person
                {
                    NameProperty = "Gøran Hansen", AgeProperty = 30, NameField = "Mr Hansen", AgeField = 32
                };
                Cross.diff(personA, personB, (left, right) =>
                {
                    var report = Diff.ObjectValues(left, right);

                    Assert.IsTrue(report.DiffFound);
                    Assert.AreEqual(4, report.Table.Rows.Count());
                    Assert.AreEqual("NameField", report.Table[0].MemberPath);
                    Assert.AreEqual("AgeField", report.Table[1].MemberPath);
                    Assert.AreEqual("NameProperty", report.Table[2].MemberPath);
                    Assert.AreEqual("AgeProperty", report.Table[3].MemberPath);
                });
            }
        }

        [TestFixture]
        public class When_diff_unequal_child_objects
        {
            [Test]
            public void It_will_report_diff_on_object_properties()
            {
                var personA = new Person
                {
                    Children = new List<Person>
                    {
                        new Person { NameProperty = "Steve" }
                    }
                };

                var personB = new Person
                {
                    Children = new List<Person>
                    {
                        new Person { NameProperty = "Bill" }                  
                    }
                };

                Cross.diff(personA, personB, (left, right) =>
                {
                    var diffResult = Diff.ObjectValues(left, right);
                    Assert.AreEqual(1, diffResult.Table.Rows.Count());
                    Assert.AreEqual("Children[0].NameProperty", diffResult.Table[0].MemberPath);
                });
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
                var steveJobs = FakeData.KnownPersons.SteveJobs;
                var clonedSteveJobs = ObjectCloner.Clone(steveJobs);

                Cross.diff(steveJobs, clonedSteveJobs, (left, right) =>
                {
                    var report = Diff.ObjectValues(left, right);
                    Assert.IsFalse(report.DiffFound);
                });
            }
        }

        [TestFixture]
        public class When_diff_object_of_different_types
        {
            [Test]
            public void It_will_rapport_diff_on_Properties()
            {
                var vehicleA = new Buss {BrandProperty = "Mercedes Benz", CapacityProperty = 22};
                var vehicleB = new Car {BrandProperty = "Mercedes Benz"};

                Cross.diff(vehicleA, vehicleB, (left, right) =>
                {
                    var report = Diff.ObjectValues(left, right);
                    Console.WriteLine(report.Print());
                    Assert.AreEqual(2, report.Table.Rows.Count());
                    Assert.AreEqual("CapacityField", report.Table.Rows.ElementAt(0).MemberPath);
                    Assert.AreEqual("CapacityProperty", report.Table.Rows.ElementAt(1).MemberPath);
                });
            }

            [Test]
            public void It_will_rapport_diff_on_Fields()
            {
                var vehicleA = new Buss { BrandField = "Mercedes Benz", CapacityField = 22 };
                var vehicleB = new Car { BrandField = "Mercedes Benz" };

                Cross.diff(vehicleA, vehicleB, (left, right) =>
                {
                    var report = Diff.ObjectValues(left, right);
                    Console.WriteLine(report.Print());
                    Assert.AreEqual(2, report.Table.Rows.Count());
                    Assert.AreEqual("CapacityField", report.Table.Rows.ElementAt(0).MemberPath);
                    Assert.AreEqual("CapacityProperty", report.Table.Rows.ElementAt(1).MemberPath);
                });
            }

            class Car
            {
                public string BrandProperty { get; set; }
                public string BrandField;
            }

            class Buss
            {
                public string BrandProperty { get; set; }
                public int CapacityProperty { get; set; }
                public string BrandField;
                public int CapacityField;
            }
        }

    }
}
