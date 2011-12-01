using System;
using System.Linq;
using NUnit.Framework;
using ODiff.Tests.Fakes;

namespace ODiff.Tests
{
    public class Diff_nulls
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
        public class When_diff_graphs_containing_null_values
        {
            [Test]
            public void It_will_report_diff_on_properties_with_nulls()
            {
                var left = new Person { NameProperty = "Gøran" };
                var right = new Person { NameProperty = null };

                var diff = Diff.ObjectValues(left, right);

                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("NameProperty", diff.Table[0].MemberPath);
            }

            [Test]
            public void It_will_report_diff_on_fields_with_nulls()
            {
                var left = new Person { NameField = "Steve" };
                var right = new Person { NameField = null };

                var diff = Diff.ObjectValues(left, right);

                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("NameField", diff.Table[0].MemberPath);
            }
        }
    }
}
