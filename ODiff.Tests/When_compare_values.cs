using NUnit.Framework;

namespace ODiff.Tests
{
    class When_compare_values
    {
        [TestFixture]
        public class When_compare_with_nulls
        {
            [Test]
            public void It_will_report_diff_if_one_value_is_null()
            {
                Assert.IsTrue(Diff.ObjectValues(1, null).DiffFound);
                Assert.IsTrue(Diff.ObjectValues(null, 1).DiffFound);
            }
        }

    }
}
