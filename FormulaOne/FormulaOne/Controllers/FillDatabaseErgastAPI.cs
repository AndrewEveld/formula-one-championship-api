using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using FormulaOne.ErgastApiModels;
using Microsoft.EntityFrameworkCore;


namespace FormulaOne.Controllers
{
    public class FillDatabaseErgastAPI
    {
        private readonly formula1APIContext _context;
        private readonly DataRepository _dataRepository;
        public FillDatabaseErgastAPI(formula1APIContext context, DataRepository dataRepository)
        {
            _context = context;
            _dataRepository = dataRepository;
        }
        private static HttpClient client;

        public static async Task<int> GetSeasonNumRounds(int season)
        {
            ErgastApiData seasonApiData = await GetApiData(season);
            return Int32.Parse(seasonApiData.Response.Total);
        }

        public async Task FillDatabase()
        {
            Race lastRecordedRace;
            if (await _context.Races.AnyAsync())
            {
                lastRecordedRace = await _dataRepository.GetMostRecentRace();
            } else
            {
                lastRecordedRace = new();
                lastRecordedRace.Season = 1950;
                lastRecordedRace.Round = 0;
            }
            int maxYear = 2021;
            int roundNumber = (int)lastRecordedRace.Round;
            for (int i = ((int)lastRecordedRace.Season); i <= maxYear; i++)
            {
                int numRounds = await GetSeasonNumRounds(i);
                for (int j = roundNumber; j <= numRounds; j++)
                {
                    ErgastApiData apiData = await GetApiData(i, round: j);
                    await InsertRaceData(apiData.Response.RaceTable.Races[0]);
                }
                roundNumber = 1;
            }
        }

        public async Task InsertRaceData(ErgastApiRace race)
        {
            Race newRace = new();
            newRace.Location = race.Circuit.CircuitName;
            newRace.Round = long.Parse(race.Round);
            newRace.Season = long.Parse(race.Season);
            Race raceDatabase = await _dataRepository.AddRaceIfAbsent(newRace);
            foreach (var result in race.Results)
            {
                Driver driverAPI = new();
                driverAPI.DateOfBirth = result.Driver.DateOfBirth;
                driverAPI.FirstName = result.Driver.GivenName;
                driverAPI.Surname = result.Driver.FamilyName;
                Driver driverDatabase = await _dataRepository.AddDriverIfAbsent(driverAPI);

                RaceResult raceResult = new();
                raceResult.DriverId = driverDatabase.Id;
                raceResult.Position = int.Parse(result.Position);
                raceResult.RaceId = raceDatabase.Id;
                await _dataRepository.AddRaceResult(raceResult);

            }
        }

        


        /// <summary>
        /// Returns Api data queried from the Ergast Api.
        /// </summary>
        /// <param name="season">Year of the season being queried.</param>
        /// <param name="round">Optional parameter. If valid round is included
        /// in request, then the race results will be returned. </param>
        /// <returns></returns>
        public static async Task<ErgastApiData> GetApiData(int season, int round = -1)
        {
            string requestUrl = round == -1 ?
                APISeasonRequest(season) : APIResultsRequest(season, round);
            client = new HttpClient();
            var productValue = new ProductInfoHeaderValue("ChampionshipAPI", "1.0");
            client.DefaultRequestHeaders.UserAgent.Add(productValue);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Task<System.IO.Stream> stringTask;
            ErgastApiData seasonData = new();
            try
            {
                stringTask = client.GetStreamAsync(requestUrl);
                seasonData = await JsonSerializer.DeserializeAsync<ErgastApiData>(await stringTask);
            }
            catch (Exception e)
            {
                Console.Write(e.Message + " : " + e.Source);
            }

            return seasonData;
        }

        public static string APIResultsRequest(int season, int round)
        {
            string request = $"http://ergast.com/api/f1/{season}/{round}/results.json";
            return request;
        }

        public static string APISeasonRequest(int season)
        {
            string request = $"http://ergast.com/api/f1/{season}.json";
            return request;
        }
    }
}
