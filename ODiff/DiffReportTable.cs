using System.Collections.Generic;
using System.Linq;

namespace ODiff
{
    public class DiffReportTable  
    {
        private readonly List<DiffReportTableRow> rows = new List<DiffReportTableRow>();
        private readonly List<IDiffFilter> filters = new List<IDiffFilter>();

        public DiffReportTable()
        {
            filters.Add(new NoFilter());
        }

        public DiffReportTableRow this[int index]
        {
            get { return Rows.ElementAt(index); }
        }

        public IEnumerable<DiffReportTableRow> Rows
        {
            get { return filterRows(); }
        }

        private IEnumerable<DiffReportTableRow> filterRows()
        {
            return rows.Where(row => filters.All(filter => filter.Include(row)));
        }

        internal void AddRow(string property, object leftValue, object rightValue)
        {
            rows.Add(new DiffReportTableRow(property, leftValue, rightValue));
        }

        internal void AddRows(DiffReportTable table)
        {
            foreach (var row in table.rows)
            {
                rows.Add(row);
            }
        }

        internal void SetFilter(params IDiffFilter[] newFilters)
        {
            filters.AddRange(newFilters);
        }
    }
}
