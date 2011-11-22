using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ODiff.Extensions;

namespace ODiff
{
    internal class ObjectGraphDiff
    {
        private readonly object leftRoot;
        private readonly object rightRoot;
        private readonly List<INodeInterceptor> nodeInterceptors = new List<INodeInterceptor>();
        private readonly DiffReport report;

        public ObjectGraphDiff(Object leftRoot, Object rightRoot, params INodeInterceptor[] interceptors)
        {
            report = NoDiffFound();
            this.rightRoot = rightRoot;
            this.leftRoot = leftRoot;
            nodeInterceptors.AddRange(interceptors);
        }

        public DiffReport Diff()
        {
            VisitNode("", leftRoot, rightRoot);
            return report;
        }

        private void VisitNode(string memberPath, object left, object right)
        {
            left = InterceptNode(memberPath, left);
            right = InterceptNode(memberPath, right);

            if (left == null && right == null) 
                return;

            if (left == null || right == null)
            {
                report.ReportDiff(memberPath, left, right);
                return;
            }

            if (left.IsAValue() || right.IsAValue())
            {
                if (!AreEqual(left, right))
                    report.ReportDiff(memberPath, left, right);
                return;
            }

            if (left.IsEnumerable() && right.IsEnumerable())
                VisitElementsInList(memberPath, left as IEnumerable, right as IEnumerable);
            VisitPublicFields(memberPath, left, right);
            VisitPublicProperties(memberPath, left, right);
        }

        private object InterceptNode(string memberPath, object node)
        {
            var interceptorsToRun = nodeInterceptors.Where(interceptor => interceptor.Use(memberPath, node));
            var result = node;
            foreach (var nodeInterceptor in interceptorsToRun)
            {
                result = nodeInterceptor.Intercept(result);
            }
            return result;
        }

        private void VisitElementsInList(string currentMemberPath, IEnumerable leftList, IEnumerable rightList)
        {
            var leftCount = leftList.Count();
            var rightCount = rightList.Count();

            var largestCount = Math.Max(leftCount, rightCount);
            var leftEnumerator = leftList.GetEnumerator();
            var rightEnumerator = rightList.GetEnumerator();

            for (var i = 0; i < largestCount; i++)
            {
                var leftValue = GetNextValueOrNullFromList(leftEnumerator, i, leftCount);
                var rightValue = GetNextValueOrNullFromList(rightEnumerator, i, rightCount);

                var newMemberPath = currentMemberPath + "[" + i + "]";

                if (leftValue == null || rightValue == null)
                {
                    var missingItemReport = new DiffReport();
                    missingItemReport.ReportDiff(newMemberPath, leftValue, rightValue);
                    report.Merge(missingItemReport);
                }
                else
                    VisitNode(newMemberPath, leftValue, rightValue);
            }
        }

        private static object GetNextValueOrNullFromList(IEnumerator enumerator, int currentIndex, int enumeratorCount)
        {
            object currentValue;
            if (currentIndex < enumeratorCount)
            {
                enumerator.MoveNext();
                currentValue = enumerator.Current;
            }
            else
                currentValue = null;
            return currentValue;
        }

        private void VisitPublicFields(string currentMemberPath, object leftObject, object rightObject)
        {
            var leftFields = leftObject.PublicFields();
            var rightFields = rightObject.PublicFields();

            for (var i = 0; i < leftFields.Length; i++)
            {
                var fieldName = leftFields[i].Name;
                var leftValue = leftFields[i].GetValue(leftObject);
                var rightValue = rightFields[i].GetValue(rightObject);

                var newMemberPath = NewPath(currentMemberPath, fieldName);
                VisitNode(newMemberPath, leftValue, rightValue);
            }
        }

        private void VisitPublicProperties(string currentMemberPath, object leftObject, object rightObject)
        {
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
                    VisitNode(newMemberPath, leftValue, rightValue);
                }
            }
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

        private static string NewPath(string currentPath, string name)
        {
            var prefixMember = (currentPath != "") ? currentPath + "." : "";
            return prefixMember + name;
        }

        private static DiffReport NoDiffFound()
        {
            return new DiffReport();
        }
    }
}
