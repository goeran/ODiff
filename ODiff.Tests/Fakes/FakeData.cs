using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ODiff.Tests.Fakes;

namespace ODiff.Tests
{
    public class FakeData
    {
        public static KnownPersons KnownPersons
        {
            get
            {
                return new KnownPersons();
            }
        }
    }
}
