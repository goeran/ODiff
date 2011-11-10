namespace ODiff
{
    public class DiffReport
    {
        private readonly ReportTable table = new ReportTable();

        public DiffReport()
        {
        }

        public DiffReport(bool diffFound)
        {
            DiffFound = diffFound;
        }

        public bool DiffFound { get; private set; }

        public ReportTable Table
        {
            get { return table; }
        }

        public void Merge(DiffReport anotherResult)
        {
            if (!DiffFound)
            {
                DiffFound = anotherResult.DiffFound;
            }
            table.AddRows(anotherResult.Table);
        }

        public void ReportDiff(string property, object leftValue, object rightValue)
        {
            table.AddRow(property, leftValue, rightValue);
        }
    }
}
