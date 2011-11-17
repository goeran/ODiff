namespace ODiff
{
    public class Diff
    {
        public static DiffReport ObjectValues(object left, object right)
        {
            return new ObjectGraphDiff(left, right).Diff();
        }
    }
}
