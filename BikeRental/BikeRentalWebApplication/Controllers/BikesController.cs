using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeRentalWebApplication.Data;
using BikeRentalWebApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BikeRentalWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BikesController : ControllerBase
    {

        private readonly BikeRentalDbContext _context;

        public BikesController(BikeRentalDbContext context)
        {
            _context = context;
        }

        // GET: api/Bikes
        // Only active Bikes!
        [HttpGet]
        [Route("{sortedBy?}")]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes(string sortedBy)
        {

            switch (sortedBy)
            {
                case "PriceOfFirstHour":
                    return await _context.Bikes
                        .Where(b => _context.Rentals.Find(b.BikeId) == null ||
                                _context.Rentals.Find(b.BikeId).Paid ||
                                _context.Rentals.Find(b.BikeId).TotalCost == 0)
                        .OrderBy(b => b.PriceFirstHour).ToListAsync();
                case "PriceOfAdditionalHours":
                    return await _context.Bikes
                        .Where(b => _context.Rentals.Find(b.BikeId) == null ||
                                _context.Rentals.Find(b.BikeId).Paid ||
                                _context.Rentals.Find(b.BikeId).TotalCost == 0)
                        .OrderBy(b => b.PriceAdditionalHours).ToListAsync();
                case "PurchaseDate":
                    return await _context.Bikes
                        .Where(b => _context.Rentals.Find(b.BikeId) == null ||
                                _context.Rentals.Find(b.BikeId).Paid ||
                                _context.Rentals.Find(b.BikeId).TotalCost == 0)
                        .OrderByDescending(b => b.PurchesDate).ToListAsync();
                default:
                    return await _context.Bikes
                        .Where(b => _context.Rentals.Find(b.BikeId) == null ||
                                _context.Rentals.Find(b.BikeId).Paid ||
                                _context.Rentals.Find(b.BikeId).TotalCost == 0).ToListAsync();
            }


        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);

            if (bike == null)
            {
                return NotFound();
            }

            return bike;
        }

        // PUT: api/Bikes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, Bike bike)
        {
            if (id != bike.BikeId)
            {
                return BadRequest();
            }

            _context.Entry(bike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(id))
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

        // POST: api/Bikes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Bike>> PostBike(Bike bike)
        {
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.BikeId }, bike);
        }

        // DELETE: api/Bikes/$id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bike>> DeleteBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return bike;
        }

        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.BikeId == id);
        }
    }
}
