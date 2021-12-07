using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;

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
        [JsonIgnore]
        public long ScoringRaces { get; set; }
        [JsonIgnore]
        public List<DriverSeasonResult> ResultsPerSeason { get; set; }
        [JsonPropertyName("ScoreSumOfSeasonAverages")]
        public double ScoreSumOfSeasonAverages { get =>
                ResultsPerSeason.Sum(rps => rps.AveragePoints);
        }
        [JsonPropertyName("ScoreSumOfSeasonAveragesOnlyPointScoring")]
        public double ScoreSumOfSeasonAveragesOnlyPointScoring { get => 
                ResultsPerSeason.Sum(rps => rps.AveragePointsPointScoring);
        }
        [JsonPropertyName("TotalSeasonsRaced")]
        public long TotalSeasonsRaced { get => ResultsPerSeason.Count; }

        public ChampionshipResult()
        {
            ResultsPerSeason = new();
        }
    }

    public class DriverSeasonResult
    {
        public long NumRaces { get; set; }
        public long NumRacesPointScoring { get; set; }
        public long Season { get; set; }
        public long TotalPoints { get; set; }
        public double AveragePoints { get => NumRaces > 0 ?
                1.0 * TotalPoints / NumRaces : 0; }
        public double AveragePointsPointScoring
        { get => NumRacesPointScoring > 0 ?
                1.0 * TotalPoints / NumRacesPointScoring : 0; }
        public DriverSeasonResult(long season)
        {
            Season = season;
        }
    }
}