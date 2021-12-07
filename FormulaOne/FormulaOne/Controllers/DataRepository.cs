using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using FormulaOne.Models;

namespace FormulaOne.Controllers
{
    public class DataRepository
    {
        private readonly formula1APIContext _context;
        public DataRepository(formula1APIContext context)
        {
            _context = context;
        }

        public async Task<Race> GetMostRecentRace()
        {
            if (!await _context.Races.AnyAsync())
            {
                Race defaultFirst = new();
                defaultFirst.Round = 0;
                defaultFirst.Season = 1950;
                return defaultFirst;
            }
            long mostRecentSeason = await GetMostRecentSeason();
            long mostRecentRound = await GetLastRaceFromSeason(mostRecentSeason);
            Race latestRace = await _context.Races.FirstAsync(r =>
                r.Season == mostRecentSeason && r.Round == mostRecentRound);
            return latestRace;

        }

        public async Task<long> GetMostRecentSeason()
        {
            long mostRecentSeason = await _context.Races.MaxAsync(r => r.Season);
            return mostRecentSeason;
        }

        public async Task<long> GetLastRaceFromSeason(long season)
        {
            IQueryable<Race> racesFromSeason = _context.Races.Where(r => r.Season == season);
            long lastRound = await racesFromSeason.MaxAsync(r => r.Round);
            return lastRound;
        }

        public async Task<Driver> AddDriverIfAbsent(Driver driver)
        {
            if (await _context.Drivers.AnyAsync(d => d.FirstName == driver.FirstName && d.Surname == driver.Surname))
            {
                return await _context.Drivers.SingleAsync(d => d.FirstName == driver.FirstName && d.Surname == driver.Surname);
            }
            else
            {
                await _context.Drivers.AddAsync(driver);
                await _context.SaveChangesAsync();
                return await _context.Drivers.SingleAsync(d => d.FirstName == driver.FirstName && d.Surname == driver.Surname);
            }
        }

        public async Task<Race> AddRaceIfAbsent(Race race)
        {
            if (await _context.Races.AnyAsync(r => r.Round == race.Round && r.Season == race.Season))
            {
                return await _context.Races.SingleAsync(r => r.Round == race.Round && r.Season == race.Season);
            }
            else
            {
                await _context.Races.AddAsync(race);
                await _context.SaveChangesAsync();
                return await _context.Races.SingleAsync(r => r.Round == race.Round && r.Season == race.Season);
            }
        }

        public async Task AddRaceResult(RaceResult raceResult)
        {
            await _context.RaceResults.AddAsync(raceResult);
            await _context.SaveChangesAsync();
        }

        public async Task<Championship> CalculateChampionship(int startYear = 1950,
            int endYear = 2022, int pointSystemId = 6, bool isAverage = false,
            bool isAveragePointScoring = false)
        {
            List<Race> racesInRange =
                await GetRacesFromSeasonRange(startYear, endYear);
            List<PositionPoint> positionPoints = await GetPositionPointsForPointSystem(pointSystemId);
            Championship championshipToReturn = new();
            championshipToReturn.StartYear = startYear;
            championshipToReturn.EndYear = endYear;
            championshipToReturn.ChampionshipResults = new();
            championshipToReturn.PointSystem =
                await GetPointSystemById(pointSystemId);

            foreach (Race race in racesInRange)
            {
                await AddRaceResultsToChampionship
                    (race, positionPoints, championshipToReturn);
            }
            championshipToReturn.ChampionshipResults =
                OrderChampionshipResults(championshipToReturn.ChampionshipResults
                , isAverage: isAverage, isAveragePointScoring: isAveragePointScoring);
            return championshipToReturn;
        }

        public async Task<List<Race>> GetRacesFromSeason(int season)
        {
            return await _context.Races
                .Where(r => r.Season == season).ToListAsync();
        }

        public async Task<List<Race>> GetRacesFromSeasonRange(int startYear,
            int endYear)
        {
            return await _context.Races
                .Where(r => r.Season >= startYear && r.Season < endYear)
                .ToListAsync();
        }

        public async Task<List<PositionPoint>>
            GetPositionPointsForPointSystem(int pointSystemId)
        {
            return await _context.PositionPoints
                .Where(pp => pp.PointSystemId == pointSystemId).ToListAsync();
        }

        public async Task<PointSystem> GetPointSystemById(int id)
        {
            return await _context.PointSystems.SingleAsync(ps => ps.Id == id);
        }

        public async Task AddRaceResultsToChampionship(Race race,
            List<PositionPoint> positionPoints, Championship championship)
        {
            List<RaceResult> raceResults = await GetRaceResultsForRace(race.Id);
            foreach (RaceResult raceResult in race.RaceResults)
            {
                if (positionPoints.Any(pp => pp.Position == raceResult.Position))
                {
                    PositionPoint driversPositionPoint = positionPoints
                    .Single(pp => pp.Position == raceResult.Position);
                    await AddPointsForDriverInChampionship(raceResult.DriverId,
                        championship, driversPositionPoint.Points, race.Season);
                } else
                {
                    await IncrementDriverSeasonResult(raceResult.DriverId,
                        championship, race.Season);
                }
            }
        }

        public async Task<List<RaceResult>> GetRaceResultsForRace(long raceId)
        {
            return await _context.RaceResults.Where(rr => rr.RaceId == raceId)
                .ToListAsync();
        }

        public async Task IncrementDriverSeasonResult
            (long driverId, Championship championship, long season)
        {
            if (!championship.ChampionshipResults
                .Any(cr => cr.Driver.Id == driverId))
            {
                Driver driverToAdd = await GetDriverById(driverId);
                ChampionshipResult championshipToAdd = new();
                championshipToAdd.Driver = driverToAdd;
                championship.ChampionshipResults.Add(championshipToAdd);
            }
            ChampionshipResult driversResult =
            championship.ChampionshipResults
                    .Single(cr => cr.Driver.Id == driverId);
            if (!driversResult.ResultsPerSeason.Any(sr => sr.Season == season))
            {
                driversResult.ResultsPerSeason.Add(new(season));
            }
            DriverSeasonResult driverSeasonResult = driversResult
                .ResultsPerSeason.Single(sr => sr.Season == season);
            driverSeasonResult.NumRaces += 1;

        }

        public async Task AddPointsForDriverInChampionship
            (long driverId, Championship championship, long points, long season)
        {
            if (!championship.ChampionshipResults
                .Any(cr => cr.Driver.Id == driverId))
            {
                Driver driverToAdd = await GetDriverById(driverId);
                ChampionshipResult championshipToAdd = new();
                championshipToAdd.Driver = driverToAdd;
                championship.ChampionshipResults.Add(championshipToAdd);
            }
            ChampionshipResult driversResult = 
            championship.ChampionshipResults
                    .Single(cr => cr.Driver.Id == driverId);
            if (!driversResult.ResultsPerSeason.Any(sr => sr.Season == season))
            {
                driversResult.ResultsPerSeason.Add(new(season));
            }
            DriverSeasonResult driverSeasonResult = driversResult
                .ResultsPerSeason.Single(sr => sr.Season == season);
            driverSeasonResult.TotalPoints += points;
            driverSeasonResult.NumRaces += 1;
            driverSeasonResult.NumRacesPointScoring += 1;
            driversResult.PointsTotal += points;
            driversResult.ScoringRaces += 1;
        }

        public async Task<Driver> GetDriverById(long id)
        {
            return await _context.Drivers.SingleAsync(d => d.Id == id);
        }

        public List<ChampionshipResult> OrderChampionshipResults
            (List<ChampionshipResult> championshipResults, bool isAverage = false,
            bool isAveragePointScoring = false)
        {
            if (isAverage)
            {
                championshipResults = championshipResults
                    .OrderByDescending(cr => cr.ScoreSumOfSeasonAverages).ToList();
            } else if (isAveragePointScoring)
            {
                championshipResults = championshipResults
                    .OrderByDescending(cr =>
                    cr.ScoreSumOfSeasonAveragesOnlyPointScoring).ToList();
            } else
            {
                championshipResults = championshipResults
                .OrderByDescending(cr => cr.PointsTotal).ToList();
            }
            for (int i = 1; i <= championshipResults.Count; i++)
            {
                championshipResults[i - 1].Position = i;
            }
            return championshipResults;
        }

    }
}
