using System;
using System.Collections;
using ODiff.Extensions;

namespace ODiff
{
    internal class ObjectGraphDiff
    {
        private readonly object left;
        private readonly object right;
        private string currentMemberName = "obj";

        public ObjectGraphDiff(Object left, Object right)
        {
            this.right = right;
            this.left = left;
        }

        public DiffReport Diff()
        {
            return CompareObject(left, right);
        }

        private DiffReport CompareObject(object leftObject, object rightObject)
        {
            if (leftObject == null && rightObject == null) return NoDiffFound();
            if (leftObject == null || rightObject == null) return new DiffReport(diffFound: true);

            var report = new DiffReport();

            if (leftObject.IsList() &&
                rightObject.IsList())
            {
                report.Merge(CompareList(leftObject as IList, rightObject as IList));
            }
            report.Merge(ComparePublicFields(leftObject, rightObject));
            report.Merge(CompareGetterProperties(leftObject, rightObject));
            return report;
        }

        private DiffReport CompareList(IList leftList, IList rightList)
        {
            var report = new DiffReport();
            var largestList = leftList.Count >= rightList.Count ? leftList : rightList;
            for (int i = 0; i < largestList.Count; i++)
            {
                var leftValue = i >= leftList.Count ? null : leftList[i];
                var rightValue = i >= rightList.Count ? null : rightList[i];
                if (leftValue.IsPrimitiveValueOrString() &&
                    !AreEqual(leftValue, rightValue))
                {
                        var listItemReport = new DiffReport(diffFound: true);
                        listItemReport.ReportDiff(currentMemberName + "[" + i + "]", leftList[i], leftList[i]);
                        report.Merge(listItemReport);
                }
                else if (leftValue == null || rightValue == null)
                {
                    var missingItemReport = new DiffReport(diffFound: true);
                    missingItemReport.ReportDiff(currentMemberName + "[" + i + "]", leftValue, rightValue);
                    report.Merge(missingItemReport);
                }
                else
                {
                    var previousMemberName = currentMemberName;
                    currentMemberName = currentMemberName + "[" + i + "]";
                    report.Merge(CompareObject(leftValue, rightValue));
                    currentMemberName = previousMemberName;
                }
            }
            return report;
        }

        private static DiffReport NoDiffFound()
        {
            return new DiffReport(diffFound: false);
        }

        private DiffReport ComparePublicFields(object leftObject, object rightObject)
        {
            var diffReport = new DiffReport();
            var leftFields = leftObject.PublicFields();
            var rightFields = rightObject.PublicFields();

            for (var i = 0; i < leftFields.Length; i++)
            {
                var fieldName = leftFields[i].Name;
                var leftValue = leftFields[i].GetValue(leftObject);
                var rightValue = rightFields[i].GetValue(rightObject);

                diffReport.Merge(CompareValue(leftValue, rightValue, fieldName));
            }
            return diffReport;
        }

        private DiffReport CompareValue(object leftValue, object rightValue, string fieldName)
        {
            var report = new DiffReport();
            if (leftValue.IsPrimitiveValueOrString() &&
                rightValue.IsPrimitiveValueOrString() &&
                !AreEqual(leftValue, rightValue))
            {
                var fieldReport = new DiffReport(diffFound: true);
                fieldReport.ReportDiff(currentMemberName + "." + fieldName, leftValue, rightValue);
                report.Merge(fieldReport);
            }
            else
            {
                var previousMemberName = currentMemberName;
                currentMemberName += "." + fieldName;
                report.Merge(CompareObject(leftValue, rightValue));
                currentMemberName = previousMemberName;
            }
            return report;
        }

        private static bool AreEqual(object leftValue, object rightValue)
        {
            if (leftValue == null && rightValue == null) return true;
            if (leftValue == null || rightValue == null) return false;

            if (leftValue.GetType() == typeof(int) &&
                rightValue.GetType() == typeof(int))
            {
                var leftAsInt = (int)leftValue;
                var rightAsInt = (int)rightValue;
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

        private DiffReport CompareGetterProperties(object leftObject, object rightObject)
        {
            var diffReport = new DiffReport();
            var leftGetterProps = leftObject.PublicGetterProperties();
            var rightGetterProps = rightObject.PublicGetterProperties();

            for (int i = 0; i < leftGetterProps.Length; i++)
            {
                var leftProperty = leftGetterProps[i];
                var rightProperty = rightGetterProps[i];

                if (!leftProperty.IsIndexerProperty() &&
                    !rightProperty.IsIndexerProperty())
                {
                    var leftValue = leftProperty.GetValue(leftObject);
                    var rightValue = rightProperty.GetValue(rightObject);

                    diffReport.Merge(CompareValue(leftValue, rightValue, leftProperty.Name));
                }
            }
            return diffReport;
        }
    }
}
