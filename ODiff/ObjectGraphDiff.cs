using System;
using System.Collections;
using ODiff.Extensions;

namespace ODiff
{
    internal class ObjectGraphDiff
    {
        private readonly object leftRoot;
        private readonly object rightRoot;

        public ObjectGraphDiff(Object leftRoot, Object rightRoot)
        {
            this.rightRoot = rightRoot;
            this.leftRoot = leftRoot;
        }

        public DiffReport Diff()
        {
            return VisitNode("", leftRoot, rightRoot);
        }

        private DiffReport VisitNode(string memberPath, object leftObject, object rightObject)
        {
            if (leftObject == null && rightObject == null) return NoDiffFound();
            
            var report = new DiffReport();

            if (leftObject == null || rightObject == null)
            {
                report.ReportDiff(memberPath, leftObject, rightObject);
                return report;
            }

            if (leftObject.IsValueType() || rightObject.IsValueType() ||
                leftObject.IsEnum() || rightObject.IsEnum())
            {
                if (!AreEqual(leftObject, rightObject))
                    report.ReportDiff(memberPath, leftObject, rightObject);
                return report;
            }

            if (leftObject.IsList() && rightObject.IsList())
            {
                report.Merge(VisitElementsInList(memberPath, leftObject as IList, rightObject as IList));
            }

            report.Merge(VisitPublicFields(memberPath, leftObject, rightObject));
            report.Merge(VisitPublicProperties(memberPath, leftObject, rightObject));
            return report;
        }

        private DiffReport VisitElementsInList(string currentMemberPath, IList leftList, IList rightList)
        {
            var report = new DiffReport();
            var largestList = leftList.Count >= rightList.Count ? leftList : rightList;
            for (var i = 0; i < largestList.Count; i++)
            {
                var leftValue = i >= leftList.Count ? null : leftList[i];
                var rightValue = i >= rightList.Count ? null : rightList[i];
                var newMemberPath = currentMemberPath + "[" + i + "]";

                if (leftValue == null || rightValue == null)
                {
                    var missingItemReport = new DiffReport();
                    missingItemReport.ReportDiff(newMemberPath, leftValue, rightValue);
                    report.Merge(missingItemReport);
                }
                else
                {
                    report.Merge(VisitNode(newMemberPath, leftValue, rightValue));
                }
            }
            return report;
        }

        private static DiffReport NoDiffFound()
        {
            return new DiffReport();
        }

        private DiffReport VisitPublicFields(string currentMemberPath, object leftObject, object rightObject)
        {
            var diffReport = new DiffReport();
            var leftFields = leftObject.PublicFields();
            var rightFields = rightObject.PublicFields();

            for (var i = 0; i < leftFields.Length; i++)
            {
                var fieldName = leftFields[i].Name;
                var leftValue = leftFields[i].GetValue(leftObject);
                var rightValue = rightFields[i].GetValue(rightObject);

                var newMemberPath = NewPath(currentMemberPath, fieldName);
                diffReport.Merge(VisitNode(newMemberPath, leftValue, rightValue));
            }
            return diffReport;
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

        private DiffReport VisitPublicProperties(string currentMemberPath, object leftObject, object rightObject)
        {
            var diffReport = new DiffReport();
            var leftGetterProps = leftObject.PublicGetterProperties();
            var rightGetterProps = rightObject.PublicGetterProperties();

            for (var i = 0; i < leftGetterProps.Length; i++)
            {
                var leftProperty = leftGetterProps[i];
                var rightProperty = rightGetterProps[i];

                if (!leftProperty.IsIndexerProperty() &&
                    !rightProperty.IsIndexerProperty())
                {
                    var leftValue = leftProperty.GetValue(leftObject);
                    var rightValue = rightProperty.GetValue(rightObject);

                    var newMemberPath = NewPath(currentMemberPath, leftProperty.Name);
                    diffReport.Merge(VisitNode(newMemberPath, leftValue, rightValue));
                }
            }
            return diffReport;
        }

        private static string NewPath(string currentPath, string name)
        {
            var prefixMember = (currentPath != "") ? currentPath + "." : "";
            return prefixMember + name;
        }
    }
}
