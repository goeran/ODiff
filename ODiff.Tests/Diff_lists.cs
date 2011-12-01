using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ODiff.Tests.Fakes;
using ODiff.Tests.Utils;

namespace ODiff.Tests
{
    public class Diff_lists
    {
        [TestFixture]
        public class When_diff_lists
        {
            [Test]
            public void It_will_report_diff_on_length()
            {
                var left = new List<string> { "a" };
                var right = new List<string> { "a", "b" };

                var report = Diff.ObjectValues(left, right);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(2, report.Table.Rows.Count());
                Assert.AreEqual("Count", report.Table[1].MemberPath);
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
                Assert.AreEqual("[1]", report.Table[0].MemberPath);
            }

            [Test]
            public void It_will_report_diff_on_objects_in_lists()
            {
                var left = new List<Person> { new Person { NameProperty = "Gøran" } };
                var right = new List<Person> { new Person { NameProperty = "Gøran Hansen" } };

                var report = Diff.ObjectValues(left, right);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("[0].NameProperty", report.Table[0].MemberPath);
            }

            [Test]
            public void It_will_report_diff_on_lists_in_lists()
            {
                var left = new List<List<string>> { new List<string> { "a" } };
                var right = new List<List<string>> { new List<string> { "a", "b" } };

                var report = Diff.ObjectValues(left, right);

                Assert.AreEqual(2, report.Table.Rows.Count());
                Assert.AreEqual("[0][1]", report.Table[0].MemberPath);
                Assert.AreEqual("[0].Count", report.Table[1].MemberPath);
            }
        }

        [TestFixture]
        public class When_object_properties_contains_lists
        {
            [Test]
            public void It_will_recursively_compare_all_members_of_list()
            {
                var steve = FakeData.KnownPersons.SteveJobs;
                var steveCopy = (Person)ObjectCloner.Clone(steve);
                steveCopy.Children.RemoveAt(0);

                var diff = Diff.ObjectValues(steve, steveCopy);
                Console.WriteLine(diff.Print());

                Assert.IsTrue(diff.DiffFound);
                Assert.AreEqual(11, diff.Table.Rows.Count());
                Assert.AreEqual("Children[0].NameField", diff.Table[0].MemberPath);
                Assert.AreEqual("Children[0].AgeField", diff.Table[1].MemberPath);
                Assert.AreEqual("Children[0].NameProperty", diff.Table[2].MemberPath);
                Assert.AreEqual("Children[0].AgeProperty", diff.Table[3].MemberPath);
                Assert.AreEqual("Children[1].NameField", diff.Table[4].MemberPath);
                Assert.AreEqual("Children[1].AgeField", diff.Table[5].MemberPath);
                Assert.AreEqual("Children[1].NameProperty", diff.Table[6].MemberPath);
                Assert.AreEqual("Children[1].AgeProperty", diff.Table[7].MemberPath);
                Assert.AreEqual("Children[1].Children", diff.Table[8].MemberPath);
                Assert.AreEqual("Children[2]", diff.Table[9].MemberPath);
                Assert.AreEqual("Children.Count", diff.Table[10].MemberPath);
            }

            [Test]
            public void It_will_handle_lists_of_lists()
            {
                var listOfLists = new List<List<string>>();
                listOfLists.Add(new List<string> { "one", "two", "three" });
                listOfLists.Add(new List<string> { "four", "five", "six" });
                var listOfListsCopy = (List<List<string>>)ObjectCloner.Clone(listOfLists);
                listOfListsCopy.Last().RemoveAt(0);
                listOfListsCopy.Last().Insert(0, "four edited");

                var diff = Diff.ObjectValues(listOfLists, listOfListsCopy);
                Assert.AreEqual("four edited", listOfListsCopy[1][0]);
                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("[1][0]", diff.Table[0].MemberPath);
            }
        }

        [TestFixture]
        public class When_object_fields_contains_lists
        {
            [Test]
            public void It_will_compare_all_members_in_list()
            {
                var steve = FakeData.KnownPersons.SteveJobs;
                var steveCopy = (Person)ObjectCloner.Clone(steve);
                steveCopy.Tags.Add("phones");

                var report = Diff.ObjectValues(steve, steveCopy);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(2, report.Table.Rows.Count());
                Assert.AreEqual("Tags[3]", report.Table[0].MemberPath);
                Assert.AreEqual("Tags.Count", report.Table[1].MemberPath);
            }

            [Test]
            public void It_will_compare_all_members_in_enumerable_collection()
            {
                var steve = FakeData.KnownPersons.SteveJobs;
                var steveCopy = (Person)ObjectCloner.Clone(steve);
                steveCopy.AddPhone("97122644");

                var report = Diff.ObjectValues(steve, steveCopy);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("Phones[0]", report.Table[0].MemberPath);
            }
        }
    }
}
