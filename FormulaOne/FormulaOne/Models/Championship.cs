using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FormulaOne.Models
{
    public class Championship
    {
        [JsonPropertyName("ChampionshipResults")]
        public List<ChampionshipResult> ChampionshipResults { get; set; }
        [JsonPropertyName("StartYear")]
        public int StartYear { get; set; }
        [JsonPropertyName("EndYear")]
        public int EndYear { get; set; }
        [JsonPropertyName("PointSystem")]
        public PointSystem PointSystem { get; set; }
    }
}
