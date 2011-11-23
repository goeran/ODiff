namespace ODiff.Filters
{
    public interface IDiffFilter
    {
        bool Include(DiffReportTableRow row);
    }
}

