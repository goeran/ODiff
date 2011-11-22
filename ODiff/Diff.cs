namespace ODiff
{
    public class Diff
    {
        public static DiffReport ObjectValues(object left, object right)
        {
            return new ObjectGraphDiff(left, right).Diff();
        }

        public static DiffReport ObjectValues(object left, object right, params INodeInterceptor[] interceptors)
        {
            return new ObjectGraphDiff(left, right, interceptors).Diff();
        }
    }
}
