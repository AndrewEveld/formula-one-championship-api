using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace FormulaOne.ErgastApiModels
{
    public class ErgastApiData
    {
        [JsonPropertyName("MRData")]
        public ErgastApiResonse Response { get; set; }
    }

    public class ErgastApiResonse
    {
        [JsonPropertyName("xmlns")]
        public string XMLRequest { get; set; }
        [JsonPropertyName("series")]
        public string Series { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("limit")]
        public string Limit { get; set; }
        [JsonPropertyName("offset")]
        public string Offset { get; set; }
        [JsonPropertyName("total")]
        public string Total { get; set; }
        public ErgastApiRaceTable RaceTable { get; set; }
    }

    public class ErgastApiRaceTable
    {
        [JsonPropertyName("season")]
        public string Season { get; set; }
        public List<ErgastApiRace> Races { get; set; }
        [JsonPropertyName("round")]
        public string Round { get; set; }
    }

    public class ErgastApiRace
    {
        [JsonPropertyName("season")]
        public string Season { get; set; }
        [JsonPropertyName("round")]
        public string Round { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("raceName")]
        public string RaceName { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        public ErgastApiCircuit Circuit { get; set; }
        [JsonPropertyName("Results")]
        public List<ErgastApiResult> Results { get; set; }
    }

    public class ErgastApiCircuit
    {
        [JsonPropertyName("circuitId")]
        public string CircuitId { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("circuitName")]
        public string CircuitName { get; set; }
        public ErgastApiLocation Location { get; set; }
    }

    public class ErgastApiLocation
    {
        [JsonPropertyName("lat")]
        public string Latitude { get; set; }
        [JsonPropertyName("long")]
        public string Longitude { get; set; }
        [JsonPropertyName("locality")]
        public string Locality { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    public class ErgastApiResult
    {
        [JsonPropertyName("number")]
        public string Number { get; set; }
        [JsonPropertyName("position")]
        public string Position { get; set; }
        [JsonPropertyName("positionText")]
        public string PositionText { get; set; }
        [JsonPropertyName("points")]
        public string Points { get; set; }
        [JsonPropertyName("Driver")]
        public ErgastApiDriver Driver { get; set; }
        [JsonPropertyName("Constructor")]
        public ErgastApiConstructor Constructor { get; set; }
        [JsonPropertyName("grid")]
        public string Grid { get; set; }
        [JsonPropertyName("laps")]
        public string Laps { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("Time")]
        public ErgastApiTime Time { get; set; }
    }

    public class ErgastApiDriver
    {
        [JsonPropertyName("driverId")]
        public string DriverId { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }
        [JsonPropertyName("familyName")]
        public string FamilyName { get; set; }
        [JsonPropertyName("dateOfBirth")]
        public string DateOfBirth { get; set; }
        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }
    }

    public class ErgastApiConstructor
    {
        [JsonPropertyName("constructorId")]
        public string ConstructorId { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }
    }

    public class ErgastApiTime
    {
        [JsonPropertyName("millis")]
        public string Milliseconds { get; set; }
        [JsonPropertyName("time")]
        public string Time { get; set; }
    }
}
