using System.Text.RegularExpressions;

namespace ODiff
{
    public class RegexMemberPathExcludeFilter : IDiffFilter
    {
        private Regex regex;

        public RegexMemberPathExcludeFilter(string pattern)
        {
            regex = new Regex(pattern);
        }

        public bool Include(DiffReportTableRow row)
        {
            return !regex.IsMatch(row.MemberPath);
        }
    }
}
