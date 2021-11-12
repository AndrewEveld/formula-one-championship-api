using System;
using System.Collections.Generic;

#nullable disable

namespace FormulaOne
{
    public partial class PositionPoint
    {
        public long PointSystemId { get; set; }
        public long Position { get; set; }
        public long Points { get; set; }

        public virtual PointSystem PointSystem { get; set; }
    }
}
