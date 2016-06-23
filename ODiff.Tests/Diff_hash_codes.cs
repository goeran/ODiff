using System;
using System.Linq;
using NUnit.Framework;
using ODiff.Tests.Fakes;

namespace ODiff.Tests
{
    [TestFixture]
    public class Diff_hash_codes
    {
        [Test]
        public void It_will_report_diff_when_subobject_hash_codes_are_equal()
        {
            int hts1 = new TimeSpan(65536L).GetHashCode();
            int hts2 = new TimeSpan(65536L).GetHashCode();
            Assert.IsTrue(hts1 == hts2);

            object o1 = new {
                A = "MyA",
                B = new TimeSpan(65536L),
                C = new TimeSpan(65536L)
            };
            object o2 = new {
                A = "MyB",
                B = new TimeSpan(65536L),
                C = new TimeSpan(65536L)
            };

            Assert.IsTrue(Diff.ObjectValues(o1, o2, new DiffConfig() { AllowCyclicGraph = true }).DiffFound);
        }

        [Test]
        public void It_will_report_diff_when_subsubobject_hash_codes_are_equal()
        {
            int hts1 = new TimeRange(32768L, 65536L).GetHashCode();
            int hts2 = new TimeRange(32768L, 65536L).GetHashCode();
            Assert.IsFalse(hts1 == hts2);

            object o1 = new
            {
                A = "MyA",
                B = new TimeRange(32768L, 65536L),
                C = new TimeRange(32768L, 65536L)
            };
            object o2 = new
            {
                A = "MyB",
                B = new TimeRange(32768L, 65536L),
                C = new TimeRange(32768L, 65536L)
            };

            Assert.IsTrue(Diff.ObjectValues(o1, o2, new DiffConfig() { AllowCyclicGraph = true }).DiffFound);
        }

        public class TimeRange
        {
            public TimeSpan Start;
            public TimeSpan End;
            public TimeRange(long start, long end)
            {
                Start = new TimeSpan(start);
                End = new TimeSpan(end);
            }
        }
    }
}
