using System;
using System.Linq;
using System.Text;
using ODiff.Filters;

namespace ODiff
{
    public class DiffReport
    {
        private readonly DiffReportTable table = new DiffReportTable();

        public bool DiffFound
        {
            get { return table.Rows.Count() > 0; }
        }

        public DiffReportTable Table
        {
            get { return table; }
        }

        public void Merge(DiffReport anotherResult)
        {
            table.AddRows(anotherResult.Table);
        }

        public void ReportDiff(string property, object leftValue, object rightValue)
        {
            table.AddRow(property, leftValue, rightValue);
        }

        public string Print()
        {
            var report = new StringBuilder();
            foreach (var row in Table.Rows)
            {
                var line = string.Format("{0}\t\t\tLeft={1}, Right={2}\r\n",
                    row.MemberPath,
                    Pretty(row.LeftValue),
                    Pretty(row.RightValue));
                report.Append(line);
            }
            return report.ToString();
        }

        private string Pretty(object obj)
        {
            return obj == null ? "<null>" : obj.ToString();
        }

        public void SetFilter(params IDiffFilter[] newFilters)
        {
            table.SetFilter(newFilters);
        }

        public void AssertNoDiffToBeFound()
        {
            if (DiffFound) throw new Exception("Expected no diff, but diff found");
        }

        public void AssertDiffToBeFound()
        {
            if (!DiffFound) throw new Exception("Expected diff to be found, but the objects are equal");
        }
    }
}
