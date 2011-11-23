using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ODiff.Interceptors;
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
                var left = new Person {AgeProperty = 20};
                var right = new Person {AgeProperty = 29};
                
                Assert.IsTrue(Diff.ObjectValues(left, right).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_float_properties()
            {
                var left = new Person {WeightProperty = 80};
                var right = new Person {WeightProperty = 81};

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
                var left = new Person {GenderProperty = Gender.Femal};
                var right = new Person {GenderProperty = Gender.Male};

                var report = Diff.ObjectValues(left, right);
                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("GenderProperty", report.Table[0].MemberPath);
            }

            [Test]
            public void It_will_report_diff_on_public_Enum_fields()
            {
                var left = new Person { GenderField = Gender.Femal };
                var right = new Person { GenderField = Gender.Male };

                var report = Diff.ObjectValues(left, right);
                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("GenderField", report.Table[0].MemberPath);
                
            }

            [Test]
            public void It_will_report_diff_on_public_DateTime_properties()
            {
                var left = new Person {BornProperty = new DateTime(1981, 10, 10)};
                var right = new Person {BornProperty = new DateTime(1981, 10, 11)};

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
                var b = new Person {Children = new List<Person>()};

                Assert.IsFalse(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_object_references_when_left_is_null()
            {
                var a = new Person();
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
                Assert.AreEqual("NameField", report.Table[0].MemberPath);
                Assert.AreEqual("AgeField", report.Table[1].MemberPath);
                Assert.AreEqual("NameProperty", report.Table[2].MemberPath);
                Assert.AreEqual("AgeProperty", report.Table[3].MemberPath);
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
                var left = new List<Person> {new Person {NameProperty = "Gøran"}};
                var right = new List<Person> {new Person {NameProperty = "Gøran Hansen"}};

                var report = Diff.ObjectValues(left, right);

                Assert.IsTrue(report.DiffFound);
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("[0].NameProperty", report.Table[0].MemberPath);
            }

            [Test]
            public void It_will_report_diff_on_lists_in_lists()
            {
                var left = new List<List<string>> {new List<string>{ "a"}};
                var right = new List<List<string>> {new List<string>{ "a", "b"}};

                var report = Diff.ObjectValues(left, right);

                Assert.AreEqual(2, report.Table.Rows.Count());
                Assert.AreEqual("[0][1]", report.Table[0].MemberPath);
                Assert.AreEqual("[0].Count", report.Table[1].MemberPath);
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

        [TestFixture]
        public class When_dealing_with_nulls
        {
            [Test]
            public void It_will_deal_with_nulls_on_root_obj()
            {
                const string left = "Bill";
                Object right = null;

                var diff = Diff.ObjectValues(left, right);

                Assert.IsTrue(diff.DiffFound);
                Assert.AreEqual(1, diff.Table.Rows.Count());
            }

            [Test]
            public void It_can_deal_with_nulls_on_props()
            {
                var left = new Person {NameProperty = "Gøran"};
                var right = new Person {NameProperty = null};

                var diff = Diff.ObjectValues(left, right);
                Assert.IsTrue(diff.DiffFound);
                Assert.AreEqual(1, diff.Table.Rows.Count());
            }
        }

        [TestFixture]
        public class When_intercept_nodes
        {
            [Test]
            public void It_can_intercept_based_on_member_path()
            {
                var interceptor = new MemberPathInterceptor<string>("^NameField$",
                    input => input.ToUpper());

                var left = new Person { NameField = "Gøran", NameProperty = "Gøran" };
                var right = new Person { NameField = "Torkild", NameProperty = "Torkild" };
                var diff = Diff.ObjectValues(left, right, interceptor);

                Assert.AreEqual("GØRAN", diff.Table[0].LeftValue);
                Assert.AreEqual("TORKILD", diff.Table[0].RightValue);
                Assert.AreEqual("Gøran", diff.Table[1].LeftValue);
                Assert.AreEqual("Torkild", diff.Table[1].RightValue);
            }

            [Test]
            public void It_will_not_intercept_invalid_type_when_intercept_by_member_path()
            {
                var interceptor = new MemberPathInterceptor<int>("^NameField$",
                    input => input + 10);

                var left = new Person { NameField = "Gøran", NameProperty = "Gøran" };
                var right = new Person { NameField = "Torkild", NameProperty = "Torkild" };
                
                var diff = Diff.ObjectValues(left, right, interceptor);

                Assert.AreEqual(2, diff.Table.Rows.Count());
            }

            [Test]
            public void It_can_intercept_on_node_type()
            {
                var interceptor = new TypeInterceptor<int>(input => input + 1);

                var left = new Person { NameField = "Gøran", WeightField = 70, AgeField = 20, AgeProperty = 20};
                var right = new Person { NameField = "Torkild", WeightField = 80, AgeField = 30, AgeProperty = 30};

                var diff = Diff.ObjectValues(left, right, interceptor);
                Assert.AreEqual(4, diff.Table.Rows.Count());
                Assert.AreEqual("Gøran", diff.Table[0].LeftValue);
                Assert.AreEqual("Torkild", diff.Table[0].RightValue);
                Assert.AreEqual(21, diff.Table[1].LeftValue);
                Assert.AreEqual(31, diff.Table[1].RightValue);
                Assert.AreEqual(70, diff.Table[2].LeftValue);
                Assert.AreEqual(80, diff.Table[2].RightValue);
                Assert.AreEqual(21, diff.Table[3].LeftValue);
                Assert.AreEqual(31, diff.Table[3].RightValue);
            }

            [Test]
            public void It_can_handle_a_relatively_large_object_graph()
            {
                const int generations = 1000;
                Person left = null;
                Person right = null;

                Measure("Building test set:", () =>
                {
                    left = new Person();
                    Generations(generations, left);
                    right = new Person();
                    Generations(generations, right);
                });

                Measure("Diff", () =>
                {
                    var diff = Diff.ObjectValues(left, right, new TypeInterceptor<string>(input =>
                    {
                        return input;
                    }));

                    Assert.AreEqual(0, diff.Table.Rows.Count());
                });
            }

            [Test]
            public void It_should_be_possible_to_intercept_lists()
            {
                var left = new Person();
                left.Children = new List<Person>();
                var right = new Person();
                right.Children = new List<Person>();

                var numberOfInterceptions = 0;
                var listInterceptor = new TypeInterceptor<IEnumerable<Person>>(input =>
                {
                    numberOfInterceptions++;
                    return input;
                });

                Diff.ObjectValues(left, right, listInterceptor);
                Assert.AreEqual(2, numberOfInterceptions);
            }

            private static void Generations(int depth, Person person)
            {
                if (depth == 0) return;
                var newChild = new Person();
                person.Children = new List<Person>();
                for (int i = 0; i < 100; i++)
                {
                    person.Children.Add(new Person());
                }
                Generations(depth - 1, newChild);
            }

            private static void Measure(string prefix, Action codeblock)
            {
                var start = DateTime.Now;
                codeblock();
                var end = DateTime.Now;
                Console.WriteLine("{0}: {1}", prefix, end.Subtract(start));
            }
        }

    }
}
