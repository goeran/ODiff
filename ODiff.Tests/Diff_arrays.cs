using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ODiff.Tests.Utils;

namespace ODiff.Tests
{
    class Diff_arrays
    {
        [TestFixture]
        public class When_diff_arrays
        {
            [Test]
            public void It_will_not_report_diff_if_content_is_the_same()
            {
                var lineItem = new LineItem { Product = "Product A", Total = 2.00M };
                var order = new Order() { Id = 1, LineItems = new[] { lineItem } };
                var orderCopy = (Order)ObjectCloner.Clone(order);

                Cross.diff(order, orderCopy, (left, right) =>
                {
                    var report = Diff.ObjectValues(left, right);

                    Assert.IsFalse(report.DiffFound);
                });
            }

            [Test]
            public void It_will_report_diff_if_content_is_not_the_same()
            {
                var lineItem = new LineItem { Product = "Product A", Total = 2.00M };
                var order = new Order() { Id = 1, LineItems = new[] { lineItem } };
                var orderCopy = (Order)ObjectCloner.Clone(order);
                orderCopy.LineItems.ElementAt(0).Total *= 2;

                Cross.diff(order, orderCopy, (left, right) =>
                {
                    var report = Diff.ObjectValues(left, right);

                    Assert.IsTrue(report.DiffFound);
                    Assert.AreEqual("LineItems[0].Total", report.Table.Rows.ElementAt(0).MemberPath);
                });
            }
        }

        [Serializable]
        public class LineItem
        {
            public string Product { get; set; }
            public decimal Total { get; set; }
        }

        [Serializable]
        public class Order
        {
            public int Id { get; set; }
            public IEnumerable<LineItem> LineItems { get; set; }
        }
    }
}
