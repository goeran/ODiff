using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODiff
{
    public class DiffResultTableRow
    {
        public string Property { get; set; }
        public Object LeftValue { get; set; }
        public Object RightValue { get; set; }
    }
}
