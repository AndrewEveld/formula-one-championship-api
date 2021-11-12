using System;
using System.Text.Json.Serialization;

namespace FormulaOne.Models
{ 
    public class ChampionshipResult
    {
        [JsonPropertyName("Driver")]
        public Driver Driver { get; set; }
        [JsonPropertyName("Position")]
        public long Position { get; set; }
        [JsonPropertyName("PointsTotal")]
        public long PointsTotal { get; set; }
    }
}
