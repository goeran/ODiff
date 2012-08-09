using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ODiff.Interceptors;
using ODiff.Tests.Fakes;

namespace ODiff.Tests
{
    class Graph_rewriting
    {
        [TestFixture]
        public class When_intercept_nodes
        {
            [Test]
            public void It_can_intercept_based_on_member_path()
            {
                var interceptor = new MemberPathInterceptor<string>(
                    "^NameField$",
                    input => input.ToUpper());
                var personA = new Person { NameField = "Gøran", NameProperty = "Gøran" };
                var personB = new Person { NameField = "Torkild", NameProperty = "Torkild" };

                var diff = Diff.ObjectValues(personA, personB, interceptor);

                Assert.AreEqual("GØRAN", diff.Table[0].LeftValue);
                Assert.AreEqual("TORKILD", diff.Table[0].RightValue);
                Assert.AreEqual("Gøran", diff.Table[1].LeftValue);
                Assert.AreEqual("Torkild", diff.Table[1].RightValue);
            }

            [Test]
            public void It_will_not_intercept_invalid_type_when_intercept_by_member_path()
            {
                var interceptor = new MemberPathInterceptor<int>(
                    "^NameField$",
                    input => input + 10);
                var personA = new Person { NameField = "Gøran", NameProperty = "Gøran" };
                var personB = new Person { NameField = "Torkild", NameProperty = "Torkild" };

                var diff = Diff.ObjectValues(personA, personB, interceptor);

                Assert.AreEqual(2, diff.Table.Rows.Count());
            }

            [Test]
            public void It_can_intercept_on_node_type()
            {
                var interceptor = new TypeInterceptor<int>(input => input + 1);
                var personA = new Person { NameField = "Gøran", WeightField = 70, AgeField = 20, AgeProperty = 20 };
                var personB = new Person { NameField = "Torkild", WeightField = 80, AgeField = 30, AgeProperty = 30 };

                var diff = Diff.ObjectValues(personA, personB, interceptor);

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
