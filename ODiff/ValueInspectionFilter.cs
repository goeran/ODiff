using System;
using System.Text.RegularExpressions;

namespace ODiff
{
    public class ValueInspectionFilter<T> : IDiffFilter
    {
        private readonly Func<T, T, bool> codeblock;
        private readonly Regex regex;

        public ValueInspectionFilter(string memberPathPattern, Func<T, T, bool> codeblock)
        {
            regex = new Regex(memberPathPattern);
            this.codeblock = codeblock;
        }

        public bool Include(DiffReportTableRow row)
        {
            if (!regex.IsMatch(row.MemberPath)) return true;

            if (row.LeftValue is T && row.RightValue is T)
                return codeblock((T)row.LeftValue, (T)row.RightValue);

            return true;
        }
    }
}
