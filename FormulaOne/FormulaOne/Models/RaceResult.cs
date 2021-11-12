using System;
using System.Collections.Generic;

#nullable disable

namespace FormulaOne
{
    public partial class RaceResult
    {
        public long Id { get; set; }
        public long RaceId { get; set; }
        public long DriverId { get; set; }
        public long Position { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual Race Race { get; set; }
    }
}
