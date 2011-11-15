using System.Collections.Generic;
using System.Text;

namespace ODiff
{
    public class ReportTable  
    {
        private readonly List<DiffReportTableRow> rows = new List<DiffReportTableRow>();

        public DiffReportTableRow this[int index]
        {
            get { return rows[index]; }
        }

        public IEnumerable<DiffReportTableRow> Rows
        {
            get { return rows; }
        }

        public void AddRow(string property, object leftValue, object rightValue)
        {
            rows.Add(new DiffReportTableRow(property, leftValue, rightValue));
        }

        public void AddRows(ReportTable table)
        {
            foreach (var row in table.rows)
            {
                rows.Add(row);
            }
        }

        public string Print()
        {
            var report = new StringBuilder();
            foreach (var row in Rows)
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
    }
}
