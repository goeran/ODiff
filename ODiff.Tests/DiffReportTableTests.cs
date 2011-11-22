using System.Linq;
using NUnit.Framework;

namespace ODiff.Tests
{
    public class DiffReportTableTests
    {
        [TestFixture]
        public class When_applying_filters
        {
            [Test]
            public void Its_possible_to_exclude_by_member_path()
            {
                var diff = new DiffReport();
                diff.ReportDiff("Id", 1, 2);
                diff.ReportDiff("Name", "Gøran", "Torkild");
                diff.ReportDiff("Age", 30, 40);
                
                diff.SetFilter(
                    new RegexMemberPathExcludeFilter("Id"),
                    new RegexMemberPathExcludeFilter("Age"));

                Assert.AreEqual(1, diff.Table.Rows.Count());
                Assert.AreEqual("Name", diff.Table[0].MemberPath);
            }

            [Test]
            public void Its_possible_to_filter_by_member_path_and_values()
            {
                var diff = new DiffReport();
                diff.ReportDiff("Name", "Gøran", "Torkild");
                diff.ReportDiff("Age", 30, 42);
                diff.ReportDiff("Weight", 230, 460);

                diff.SetFilter(new ValueInspectionFilter<int>("Age",
                    (leftValue, rightValue) => leftValue > 30));

                Assert.AreEqual(2, diff.Table.Rows.Count());
                Assert.AreEqual("Name", diff.Table[0].MemberPath);
                Assert.AreEqual("Weight", diff.Table[1].MemberPath);
            }

            [Test]
            public void It_is_possible_to_exclude_many_diffs_using_filters()
            {
                var diff = new DiffReport();
                diff.ReportDiff("StatusLog.LogItems[0].Timestamp", "", "");
                diff.ReportDiff("StatusLog.LogItems[1].Timestamp", "", "");
                diff.ReportDiff("JobIds[0]", 277540, 277906);
                diff.ReportDiff("JobIds[1]", 277541, 277907);
                diff.ReportDiff("JobIds[2]", 277542, 277908);
                diff.ReportDiff("CreatedBy", "toinds", "gohans");

                diff.SetFilter(new RegexMemberPathExcludeFilter(
                    "^MathModelCaseId$|" +
                    @"^CaseSnapshot\.MetaData\.Name$|" +
                    @"^CaseSnapshot\.MetaData\.Category$|" +
                    @"^Id$|\.Id$|" +
                    @"^StatusLog\.LogItems\[\d*\]\.Timestamp$|" +
                    @"^JobIds\[\d*\]$|" +
                    @"^CopyProperties.Name$|" +
                    @"^Timestamp$|" +
                    @"^CreatedBy$|" +
                    @"^CreatedWhen$"));

                Assert.AreEqual(0, diff.Table.Rows.Count());
            }
        }
    }
}
