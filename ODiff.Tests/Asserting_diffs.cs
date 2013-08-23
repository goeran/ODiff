using System;
using NUnit.Framework;

namespace ODiff.Tests
{
    class Asserting_diffs
    {
        [TestFixture]
        public class When_asserting_for_no_diff_to_be_found
        {
            [Test]
            public void It_should_not_throw_exception_when_no_diff()
            {
                Diff.ObjectValues("A", "A").AssertNoDiffToBeFound();
            }

            [Test]
            [ExpectedException(typeof(Exception), ExpectedMessage = "Expected no diff, but diff found")]
            public void It_should_throw_exception_when_diff_found()
            {
                Diff.ObjectValues("A", "B").AssertNoDiffToBeFound();
            }
        }

        [TestFixture]
        public class When_asserting_for_diff_to_be_found
        {
            [Test]
            [ExpectedException(typeof(Exception), ExpectedMessage = "Expected diff to be found, but the objects are equal")]
            public void It_should_throw_exception_when_no_diff_found()
            {
                Diff.ObjectValues("A", "A").AssertDiffToBeFound();
            }

            [Test]
            public void It_should_not_throw_exception_when_diff_found()
            {
                Diff.ObjectValues("A", "B").AssertDiffToBeFound();
            }
        }
    }
}
