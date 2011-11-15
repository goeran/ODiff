using System;

namespace ODiff
{
    public class DiffReportTableRow
    {
        public DiffReportTableRow(string memberPath, object left, object right)
        {
            MemberPath = memberPath;
            LeftValue = left;
            RightValue = right;
        }

        public string MemberPath { get; private set; }
        public Object LeftValue { get; private set; }
        public Object RightValue { get; private set; }
    }
}
