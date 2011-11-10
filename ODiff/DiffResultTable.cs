using System;
using System.Collections;
using System.Collections.Generic;

namespace ODiff
{
    public class DiffResultTable  
    {
        private readonly List<DiffResultTableRow> rows = new List<DiffResultTableRow>();

        public DiffResultTableRow this[int index]
        {
            get { return rows[index]; }
        }

        public IEnumerable<DiffResultTableRow> Rows
        {
            get { return rows; }
        }

        public void AddRow(string property, object leftValue, object rightValue)
        {
            rows.Add(new DiffResultTableRow
            {
                Property = property,
                LeftValue = leftValue,
                RightValue = rightValue
            });
        }

        public void AddRows(DiffResultTable table)
        {
            foreach (var row in table.rows)
            {
                rows.Add(row);
            }
        }
    }
}
