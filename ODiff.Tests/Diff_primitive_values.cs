using System;
using NUnit.Framework;
using ODiff.Tests.Fakes;

namespace ODiff.Tests
{
    class Diff_primitive_values
    {
        [TestFixture]
        public class When_compare_with_nulls
        {
            [Test]
            public void It_will_report_diff_if_one_value_is_null(
                [Values(1, "1", Gender.Female, 1.1f, 1.1d, true)] object value)
            {
                Assert.IsTrue(Diff.ObjectValues(value, null).DiffFound);
                Assert.IsTrue(Diff.ObjectValues(null, value).DiffFound);
            }

            [Test]
            public void It_will_not_report_diff_if_both_values_are_null()
            {
                Assert.IsFalse(Diff.ObjectValues(null, null).DiffFound);
            }
        }

        [TestFixture]
        public class When_compare_equal_values
        {
            [Test]
            public void It_will_not_report_diff(
                [Values(1, 1d, 1f, true, false, Gender.Female, 'x',
                        (sbyte)11, (byte)12, -13, (uint)14, "15")] object value)
            {
                Assert.IsFalse(Diff.ObjectValues(value, value).DiffFound);
            } 
        }
    }
}
