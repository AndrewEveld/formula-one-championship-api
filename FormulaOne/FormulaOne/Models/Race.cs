using System;
using System.Collections.Generic;

#nullable disable

namespace FormulaOne
{
    public partial class Race
    {
        public Race()
        {
            RaceResults = new HashSet<RaceResult>();
        }

        public long Id { get; set; }
        public long Season { get; set; }
        public string Location { get; set; }
        public long Round { get; set; }

        public virtual ICollection<RaceResult> RaceResults { get; set; }
    }
}
