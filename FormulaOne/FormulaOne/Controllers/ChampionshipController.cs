using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormulaOne.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChampionshipController : ControllerBase
    {
        private readonly DataRepository _dataRepository;

        public ChampionshipController(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

      

        // GET: api/Championship/2004
        [HttpGet()]
        public async Task<ActionResult<Championship>>
            GetChampionship(int startYear = 1950, int endYear = 2022,
            int pointSystemId = 6, bool isAverage = false,
            bool isAveragePointScoring = false)
        {
            return await _dataRepository
                .CalculateChampionship(startYear: startYear, endYear: endYear,
                pointSystemId: pointSystemId, isAverage: isAverage,
                isAveragePointScoring: isAveragePointScoring);
        }

        ////// GET: api/Championship/2004
        ////[HttpGet("championship")]
        ////public async Task<ActionResult<Championship>>
        ////    GetChampionship(int startYear, int pointSystem)
        ////{
        ////    return await _dataRepository
        ////        .CalculateChampionship(startYear: startYear, endYear: endYear,
        ////        pointSystem: pointSystem);
        ////}

        //private async Task<List<RaceResult>> GetDriverResults(int driverId, int startYear, int endYear)
        //{
        //    List<Race> racesInRange = await _context.Races
        //        .Where(r => r.Season >= startYear && r.Season <= endYear)
        //        .ToListAsync();
        //    List<RaceResult> raceResults = await _context.RaceResults
        //        .Where(rr => rr.DriverId == driverId && racesInRange
        //        .Any(r => r.Id == rr.RaceId))
        //        .ToListAsync();
        //    return raceResults;
        //}

        //private async Task<long> CalculatePoints(List<RaceResult> raceResults, PointSystem pointSystem)
        //{
        //    List<PositionPoint> positionPoints = await _context.PositionPoints
        //        .Where(pp => pp.PointSystemId == pointSystem.Id)
        //        .ToListAsync();
        //    long totalPoints = 0;
        //    raceResults.ForEach(rr => {
        //        if (positionPoints.Exists(pp => pp.Position == rr.Position))
        //        {
        //            totalPoints += positionPoints.Find(pp => pp.Position == rr.Position).Points;
        //        }
        //    });
        //    return totalPoints;
        //}
        // TODO Flags for comparing drivers.
        // Only count races where driver finished?
        // Average out the results for a season based on 
    }
}
