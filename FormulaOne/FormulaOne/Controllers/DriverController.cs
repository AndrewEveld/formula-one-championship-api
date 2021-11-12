using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormulaOne;

namespace FormulaOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly formula1APIContext _context;

        public DriverController(formula1APIContext context)
        {
            _context = context;
        }

        // GET: api/Driver
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        // GET: api/Driver/5
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetDriver(long id)
        {
            FillDatabaseErgastAPI ergast = new(_context, new DataRepository(_context));
            await ergast.FillDatabase();
            //var driver = await _context.Drivers.FindAsync(id);
            //int rounds = await FillDatabaseErgastAPI.GetSeasonNumRounds(1950);
            //var numDrivers = await FillDatabaseErgastAPI.GetApiData(1950, round: 1);
            //Console.WriteLine(numDrivers.Response.RaceTable.Races[0].Results[0].Driver.GivenName);
            //Console.WriteLine(rounds);
            //if (driver == null)
            //{
            //    return NotFound();
            //}

            //return driver;
            return 0;
        }

        // PUT: api/Driver/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(long id, Driver driver)
        {
            if (id != driver.Id)
            {
                return BadRequest();
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Driver
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { id = driver.Id }, driver);
        }

        // DELETE: api/Driver/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(long id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverExists(long id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
