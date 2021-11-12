using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


#nullable disable

namespace FormulaOne
{
    public partial class PointSystem
    {
        [JsonIgnore]
        public long Id { get; set; }
        public long SeasonStarted { get; set; }
        public long SeasonEnded { get; set; }
    }
}
