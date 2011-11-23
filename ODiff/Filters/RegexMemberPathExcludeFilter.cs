using System.Text.RegularExpressions;

namespace ODiff.Filters
{
    public class RegexMemberPathExcludeFilter : IDiffFilter
    {
        private readonly Regex regex;

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
