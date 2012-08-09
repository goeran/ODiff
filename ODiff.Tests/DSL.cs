using System;

namespace ODiff.Tests
{
    internal static class Cross
    {
        public static void diff(object left, object right, Action<object, object> assert)
        {
            assert(left, right);
            assert(right, left);
        }
    }
}
