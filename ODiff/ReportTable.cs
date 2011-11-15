using System.Collections.Generic;

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
    }
}
