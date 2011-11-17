namespace ODiff
{
    public interface IDiffFilter
    {
        bool Include(DiffReportTableRow row);
    }
}

