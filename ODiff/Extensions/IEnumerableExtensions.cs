using System.Collections;

namespace ODiff.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int Count(this IEnumerable collection)
        {
            int count = 0;
            
            #pragma warning disable 168
            if (collection != null)
                foreach (var obj in collection)
                    count++;
            #pragma warning restore 168

            return count;
        }
    }
}
