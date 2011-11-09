using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ODiff.Tests.Learning
{
    [TestFixture]
    public class How_to_get_members_from_and_ExpandoObject
    {
        [Test]
        public void Its_a_dictionary_so_a_dictionary_cast_and_lookup_would_do()
        {
            dynamic a = new ExpandoObject();
            a.Name = "Steve";

            var members = (IDictionary<string, object>) a;
            Assert.IsTrue(members.ContainsKey("Name"));
        }
    }
}
