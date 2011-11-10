using System;

namespace ODiff
{
    public class DiffResult
    {
        private readonly DiffResultTable table = new DiffResultTable();

        public DiffResult()
        {
        }

        public DiffResult(bool diffFound)
        {
            DiffFound = diffFound;
        }

        public bool DiffFound { get; private set; }

        public DiffResultTable Table
        {
            get { return table; }
        }

        public void Merge(DiffResult anotherResult)
        {
            if (!DiffFound)
            {
                DiffFound = anotherResult.DiffFound;
            }
            table.AddRows(anotherResult.Table);
        }

        public void Report(string property, object leftValue, object rightValue)
        {
            table.AddRow(property, leftValue, rightValue);
        }
    }
}
