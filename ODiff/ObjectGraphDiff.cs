using System;
using System.Collections;
using ODiff.Extensions;

namespace ODiff
{
    internal class ObjectGraphDiff
    {
        private readonly object leftRoot;
        private readonly object rightRoot;
        private string currentMemberPath = "";
        private string previousMemberName = "";

        public ObjectGraphDiff(Object leftRoot, Object rightRoot)
        {
            this.rightRoot = rightRoot;
            this.leftRoot = leftRoot;
        }

        public DiffReport Diff()
        {
            return VisitNode(leftRoot, rightRoot);
        }

        private DiffReport VisitNode(object leftObject, object rightObject)
        {
            if (leftObject == null && rightObject == null) return NoDiffFound();
            if (leftObject == null || rightObject == null)
            {
                var reportOnObj = new DiffReport();
                reportOnObj.ReportDiff(currentMemberPath, leftObject, rightObject);
                return reportOnObj;
            }

            var report = new DiffReport();
            if (leftObject.IsValueType() || rightObject.IsValueType() ||
                leftObject.IsEnum() || rightObject.IsEnum())
            {
                if (!AreEqual(leftObject, rightObject))
                    report.ReportDiff(currentMemberPath, leftObject, rightObject);
                return report;
            }
            if (leftObject.IsList() &&
                rightObject.IsList())
                report.Merge(CompareList(leftObject as IList, rightObject as IList));
            report.Merge(ComparePublicFields(leftObject, rightObject));
            report.Merge(CompareGetterProperties(leftObject, rightObject));
            return report;
        }

        private DiffReport CompareList(IList leftList, IList rightList)
        {
            var report = new DiffReport();
            var largestList = leftList.Count >= rightList.Count ? leftList : rightList;
            var memberPathBeforeIteration = currentMemberPath;
            for (var i = 0; i < largestList.Count; i++)
            {
                var leftValue = i >= leftList.Count ? null : leftList[i];
                var rightValue = i >= rightList.Count ? null : rightList[i];
                ObjectGraphPath(currentMemberPath + "[" + i + "]");

                if (leftValue.IsValueType() &&
                    !AreEqual(leftValue, rightValue))
                {
                    var listItemReport = new DiffReport();
                    listItemReport.ReportDiff(currentMemberPath, leftValue, rightValue);
                    report.Merge(listItemReport);
                }
                else if (leftValue == null || rightValue == null)
                {
                    var missingItemReport = new DiffReport();
                    missingItemReport.ReportDiff(currentMemberPath, leftValue, rightValue);
                    report.Merge(missingItemReport);
                }
                else
                {
                    report.Merge(VisitNode(leftValue, rightValue));
                }
                currentMemberPath = memberPathBeforeIteration;
            }
            return report;
        }

        private void ObjectGraphPath(string path)
        {
            previousMemberName = currentMemberPath;
            currentMemberPath = path;
        }

        private static DiffReport NoDiffFound()
        {
            return new DiffReport();
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

                var prev = currentMemberPath;
                var prefixMember = (currentMemberPath != "") ? currentMemberPath + "." : "";
                currentMemberPath = prefixMember + fieldName;
                diffReport.Merge(VisitNode(leftValue, rightValue));
                currentMemberPath = prev;
            }
            return diffReport;
        }

        private DiffReport Compare(object leftValue, object rightValue, string fieldName)
        {
            var report = new DiffReport();
            if (leftValue.IsValueType() ||
                rightValue.IsValueType() ||
                leftValue.IsEnum() || 
                rightValue.IsEnum())
            {
                if (!AreEqual(leftValue, rightValue))
                {
                    var fieldReport = new DiffReport();
                    var prefixMember = (currentMemberPath != "") ? currentMemberPath + "." : "";
                    fieldReport.ReportDiff(prefixMember + fieldName, leftValue, rightValue);
                    report.Merge(fieldReport);
                }
            }
            else
            {
                var previousMemberName = currentMemberPath;
                var prefixMember = (currentMemberPath != "") ? currentMemberPath + "." : "";
                currentMemberPath = prefixMember + fieldName;
                report.Merge(VisitNode(leftValue, rightValue));
                currentMemberPath = previousMemberName;
            }
            return report;
        }

        private static bool AreEqual(object leftValue, object rightValue)
        {
            if (leftValue == null && rightValue == null) return true;
            if (leftValue == null || rightValue == null) return false;

            if (leftValue.IsValueType() ||
                leftValue.IsEnum())
                return leftValue.Equals(rightValue);

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

                    var prev = currentMemberPath;
                    var prefixMember = (currentMemberPath != "") ? currentMemberPath + "." : "";
                    currentMemberPath = prefixMember + leftProperty.Name;
                    diffReport.Merge(VisitNode(leftValue, rightValue));
                    currentMemberPath = prev;
                }
            }
            return diffReport;
        }
    }
}
