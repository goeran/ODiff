using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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
            public void It_will_report_diff_on_public_int_properties()
            {
                var a = new Person {AgeProperty = 20};
                var b = new Person {AgeProperty = 29};

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_public_int_fields()
            {
                var a = new Person { AgeField = 20 };
                var b = new Person { AgeField = 29 };

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_not_report_diff_on_object_references()
            {
                var a = new Person {Assets = new List<object>()};
                var b = new Person {Assets = new List<object>()};

                Assert.IsFalse(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_object_references_when_left_is_null()
            {
                var a = new Person {};
                var b = new Person {Assets = new List<object>()};

                Assert.IsTrue(Diff.ObjectValues(a, b).DiffFound);
            }

            [Test]
            public void It_will_report_diff_on_object_references_when_right_is_null()
            {
                var a = new Person {Assets = new List<object>()};
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
                Assert.AreEqual(1, report.Table.Rows.Count());
                Assert.AreEqual("obj.Count", report.Table[0].Member);
                Assert.AreEqual(1, report.Table[0].LeftValue);
                Assert.AreEqual(2, report.Table[0].RightValue);
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

                DiffReport result = Diff.ObjectValues(a, b);
                
                Assert.AreEqual(false, result.DiffFound);
            }
        }  
      
        private class Person
        {
            public string NameProperty { get; set; }
            public int AgeProperty { get; set; }
            public List<object> Assets { get; set; }

            public string NameField;
            public int AgeField;
        }
    }
}
