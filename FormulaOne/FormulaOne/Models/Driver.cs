using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


#nullable disable

namespace FormulaOne
{
    public partial class Driver
    {
        public Driver()
        {
            RaceResults = new HashSet<RaceResult>();
        }
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }

        [JsonIgnore]
        public virtual ICollection<RaceResult> RaceResults { get; set; }
    }
}
