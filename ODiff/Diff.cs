using System;
using System.Collections;
using System.Linq;
using ODiff.Extensions;

namespace ODiff
{
    public class Diff
    {
        public static DiffResult ObjectValues(object left, object right)
        {
            if (left == null && right == null) return NoDiffFound();
            if ((left == null && right != null) ||
                (left != null && right == null)) 
                return new DiffResult(diffFound: true);

            var report = new DiffResult();

            if (left.IsList() &&
                right.IsList())
            {
                report.Merge(CompareLists(left as IList, right as IList));
            }

            report.Merge(CheckPublicFields(left, right));
            report.Merge(CheckGetterProperties(left, right));
            return report;
        }

        private static DiffResult CompareLists(IList left, IList right)
        {
            var report = new DiffResult();
            for (int i = 0; i < left.Count; i++)
            {
                if (!AreEqual(left[i], right[i]))
                {
                    var listItemReport = new DiffResult(diffFound: true);
                    listItemReport.Report("obj[" + i + "]", left[i], left[i]);
                    report.Merge(listItemReport);
                }
            }
            return report;
        }

        private static DiffResult NoDiffFound()
        {
            return new DiffResult(diffFound: false);
        }

        private static DiffResult CheckPublicFields(object left, object right)
        {
            var leftFields = left.PublicFields();
            var rightFields = right.PublicFields();

            for (int i = 0; i < leftFields.Length; i++)
            {
                var leftValue = leftFields[i].GetValue(left);
                var rightValue = rightFields[i].GetValue(right);

                if (leftValue != null && rightValue != null &&
                    !AreEqual(leftValue, rightValue))
                    return new DiffResult(diffFound: true);
            }

            return NoDiffFound();
        }

        private static bool AreEqual(object leftValue, object rightValue)
        {
            if (leftValue == null && rightValue == null) return true;
            if (leftValue == null && rightValue != null) return false;
            if (leftValue != null && rightValue == null) return false;

            if (leftValue.GetType() == typeof(int) &&
                rightValue.GetType() == typeof(int))
            {
                var leftAsInt = (int) leftValue;
                var rightAsInt = (int) rightValue;
                return leftAsInt == rightAsInt;
            }
            if (leftValue.GetType() == typeof(string) &&
                rightValue.GetType() == typeof(string))
                return leftValue.Equals(rightValue);
            if (leftValue.GetType().IsPrimitive)
                return leftValue.Equals(rightValue);
            if (leftValue.GetType() == rightValue.GetType())
                return true;

            return true; 
        }

        private static DiffResult CheckGetterProperties(object left, object right)
        {
            var leftGetterProps = left.PublicGetterProperties();
            var rightGetterProps = right.PublicGetterProperties();

            for (int i = 0; i < leftGetterProps.Length; i++)
            {
                var leftProperty = leftGetterProps[i];
                var rightProperty = rightGetterProps[i];

                if (!leftProperty.IsIndexerProperty() &&
                    !rightProperty.IsIndexerProperty())
                {
                    var leftValue = leftProperty.GetValue(left);
                    var rightValue = rightProperty.GetValue(right);

                    if (!AreEqual(leftValue, rightValue))
                    {
                        var report = new DiffResult(diffFound: true);
                        report.Report("obj." + leftGetterProps[i].Name, leftValue, rightValue);
                        return report;
                    }
                }
            }

            return NoDiffFound();
        }
    }
}
