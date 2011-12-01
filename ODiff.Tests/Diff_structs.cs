using System.Linq;
using NUnit.Framework;

namespace ODiff.Tests
{
    public class Diff_structs
    {
        [TestFixture]
        public class When_diff_structs
        {
            [Test]
            public void It_will_detect_diff_on_public_properties()
            {
                var left = new Employee { Name = "Steve" };
                var right = new Employee();

                var diff = Diff.ObjectValues(left, right);

                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("Name", diff.Table[0].MemberPath);
            }

            [Test]
            public void It_will_detect_diff_on_public_fields()
            {
                var left = new Employee { Number = 1 };
                var right = new Employee { Number = 2 };

                var diff = Diff.ObjectValues(left, right);

                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("Number", diff.Table[0].MemberPath);
            }

            struct Employee
            {
                public int Number;
                public string Name { get; set; }
            }
        }
    }
}
