using System.Collections.Generic;
using NUnit.Framework;
using ODiff.Extensions;

namespace ODiff.Tests.Learning
{
    public class Learning_about_reflection
    {
        [Test]
        public void Is_a_x()
        {
            Assert.IsTrue(new CustomList().IsList());
        }

        private class CustomList : List<string>
        {
            
        }
    }
}
