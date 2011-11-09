using System;
using System.Reflection;

namespace ODiff
{
    public class Diff
    {
        private static readonly DiffResult NoDiffFound = new DiffResult(diffFound: false);

        public static DiffResult Object(object left, object right)
        {
            var result = CheckPublicFields(left, right);
            result.Merge(CheckGetterProperties(left, right));
            return result;
        }

        private static DiffResult CheckPublicFields(object left, object right)
        {
            var leftFields = PublicFields(left);
            var rightFields = PublicFields(right);

            for (int i = 0; i < leftFields.Length; i++)
            {
                var leftValue = leftFields[i].GetValue(left);
                var rightValue = rightFields[i].GetValue(right);

                if (leftValue != null && rightValue != null &&
                    AreEqual(leftValue, rightValue))
                    return new DiffResult(diffFound: true);
            }

            return NoDiffFound;
        }

        private static bool AreEqual(object leftValue, object rightValue)
        {
            if (leftValue.GetType() == typeof(int) &&
                rightValue.GetType() == typeof(int))
            {
                var leftAsInt = (int) leftValue;
                var rightAsInt = (int) rightValue;
                return leftAsInt != rightAsInt;
            }
            if (leftValue.GetType().IsPrimitive)
                return !leftValue.Equals(rightValue);
            
            return false; //object ref to an another object
        }

        private static FieldInfo[] PublicFields(object left)
        {
            return left.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        }

        private static DiffResult CheckGetterProperties(object left, object right)
        {
            var leftGetterProps = PublicGetterProperties(left);
            var rightGetterProps = PublicGetterProperties(right);

            for (int i = 0; i < leftGetterProps.Length; i++)
            {
                var leftValue = leftGetterProps[i].GetValue(left, new object[] { });
                var rightValue = rightGetterProps[i].GetValue(right, new object[] {});

                if (leftValue != null && rightValue != null &&
                    AreEqual(leftValue, rightValue))
                    return new DiffResult(diffFound: true);
            }

            return NoDiffFound;
        }

        private static PropertyInfo[] PublicGetterProperties(object left)
        {
            return left.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
        }
    }
}
