﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ODiff.Tests.Fakes;
using ODiff.Tests.Utils;

namespace ODiff.Tests
{
    public class Compare_two_objects
    {
        [TestFixture]
        public class When_diff_objects_and_null
        {
            [Test]
            public void It_will_report_diff_when_left_is_null()
            {
                Object a = null;
                var b = new Person();

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_when_right_is_null()
            {
                var a = new Person();
                Object b = null;

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_not_report_diff_when_both_null()
            {
                Object a = null;
                Object b = null;

                Assert.IsFalse(Diff.ObjectValues(a, b).DiffFound);
            }
        }

        [TestFixture]
        public class When_diff_uneqal_objects
        {
            [Test]
            public void It_will_report_diff_on_public_String_properties()
            {
                var a = new Person {NameProperty = "Gøran"};
                var b = new Person {NameProperty = "Gøran Hansen"};

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_String_fields()
            {
                var a = new Person {NameField = "Steve"};
                var b = new Person {NameField = "Bill"};

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_decimal_properties()
            {
                var a = new Person {HeightProperty = 1.8m};
                var b = new Person {HeightProperty = 2.10m};

                var diff = Diff.ObjectValues(a, b);
                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("obj.HeightProperty", diff.Table[0].Member);
            }

            [Test]
            public void It_will_report_diff_on_public_decimal_fields()
            {
                var a = new Person { HeightField = 1.8m };
                var b = new Person { HeightField = 2.10m };

                var diff = Diff.ObjectValues(a, b);
                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("obj.HeightField", diff.Table[0].Member);
            }

            [Test]
            public void It_will_report_diff_on_public_int_fields()
            {
                var a = new Person { AgeField = 20 };
                var b = new Person { AgeField = 29 };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_float_properties()
            {
                
            }

            [Test]
            public void It_will_report_diff_on_public_Enum_properties()
            {
                var left = new Person {GenderProperty = Gender.Femal};
                var right = new Person {GenderProperty = Gender.Male};

                var report = Diff.ObjectValues(left, right);
                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("obj.GenderProperty", report.Table[0].Member);
            }

            [Test]
            public void It_will_report_diff_on_public_Enum_fields()
            {
                var left = new Person { GenderField = Gender.Femal };
                var right = new Person { GenderField = Gender.Male };

                var report = Diff.ObjectValues(left, right);
                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("obj.GenderField", report.Table[0].Member);
                
            }

            [Test]
            public void It_will_not_report_diff_on_object_references()
            {
                var a = new Person { Children = new List<Person>() };
                var b = new Person {Children = new List<Person>()};

                Assert.IsFalse(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_object_references_when_left_is_null()
            {
                var a = new Person {};
                var b = new Person {Children = new List<Person>()};

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
                    NameProperty = "Gøran", AgeProperty = 29,
                    NameField = "Hansen", AgeField = 31
                };
                var right = new Person
                {
                    NameProperty = "Gøran Hansen", AgeProperty = 30,
                    NameField = "Mr Hansen", AgeField = 32
                };

                var report = Diff.ObjectValues(left, right);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(4, report.Table.Rows.Count());
                Assert.AreEqual("obj.NameField", report.Table[0].Member);
                Assert.AreEqual("obj.AgeField", report.Table[1].Member);
                Assert.AreEqual("obj.NameProperty", report.Table[2].Member);
                Assert.AreEqual("obj.AgeProperty", report.Table[3].Member);
            }
        }

        [TestFixture]
        public class When_diff_lists
        {
            [Test]
            public void It_will_report_diff_on_length()
            {
                var left = new List<string> {"a"};
                var right = new List<string> {"a", "b"};

                var report = Diff.ObjectValues(left, right);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(2, report.Table.Rows.Count());
                Assert.AreEqual("obj.Count", report.Table[1].Member);
                Assert.AreEqual(1, report.Table[1].LeftValue);
                Assert.AreEqual(2, report.Table[1].RightValue);
            }

            [Test]
            public void It_will_report_diff_on_content_in_lists()
            {
                var left = new List<string> { "a", "a" };
                var right = new List<string> { "a", "b" };

                var report = Diff.ObjectValues(left, right);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("obj[1]", report.Table[0].Member);
            }

            [Test]
            public void It_will_report_diff_on_objects_in_lists()
            {
                var left = new List<Person> {new Person {NameProperty = "Gøran"}};
                var right = new List<Person> {new Person {NameProperty = "Gøran Hansen"}};

                var report = Diff.ObjectValues(left, right);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("obj[0].NameProperty", report.Table[0].Member);
            }

            [Test]
            public void It_will_report_diff_on_lists_in_lists()
            {
                var left = new List<List<string>> {new List<string>{ "a"}};
                var right = new List<List<string>> {new List<string>{ "a", "b"}};

                var report = Diff.ObjectValues(left, right);

                Assert.AreEqual(2, report.Table.Rows.Count());
                Assert.AreEqual("obj[0][1]", report.Table[0].Member);
                Assert.AreEqual("obj[0].Count", report.Table[1].Member);
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
        public class When_object_properties_contains_lists
        {
            [Test]
            public void It_will_recursively_compare_all_members_of_list()
            {
                var steve = FakeData.KnownPersons.SteveJobs;
                var steveCopy = ObjectCloner.Clone(steve) as Person;
                steveCopy.Children.RemoveAt(0);

                var report = Diff.ObjectValues(steve, steveCopy);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(6, report.Table.Rows.Count());
                Assert.AreEqual("obj.Children[0].NameField", report.Table[0].Member);
                Assert.AreEqual("obj.Children[0].AgeField", report.Table[1].Member);
                Assert.AreEqual("obj.Children[0].NameProperty", report.Table[2].Member);
                Assert.AreEqual("obj.Children[0].AgeProperty", report.Table[3].Member);
                Assert.AreEqual("obj.Children[1]", report.Table[4].Member);
                Assert.AreEqual("obj.Children.Count", report.Table[5].Member);
            }            
        }

        [TestFixture]
        public class When_object_fields_contains_lists
        {
            [Test]
            public void It_will_compare_all_members_in_list()
            {
                var steve = FakeData.KnownPersons.SteveJobs;
                var steveCopy = ObjectCloner.Clone(steve) as Person;
                steveCopy.Tags.Add("phones");

                var report = Diff.ObjectValues(steve, steveCopy);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(2, report.Table.Rows.Count());
                Assert.AreEqual("obj.Tags[3]", report.Table[0].Member);
                Assert.AreEqual("obj.Tags.Count", report.Table[1].Member);
            }
        }
    }
}
