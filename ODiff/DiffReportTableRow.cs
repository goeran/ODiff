using System;

namespace ODiff
{
    public class DiffReportTableRow
    {
        public string Property { get; set; }
        public Object LeftValue { get; set; }
        public Object RightValue { get; set; }
    }
}
