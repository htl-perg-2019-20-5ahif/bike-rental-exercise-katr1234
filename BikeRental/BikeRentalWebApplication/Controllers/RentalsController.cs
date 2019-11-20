using BikeRentalWebApplication.Data;
using BikeRentalWebApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly BikeRentalDbContext _context;

        public RentalsController(BikeRentalDbContext context)
        {
            _context = context;
        }

        // GET: api/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            return await _context.Rentals.Include(r => r.Customer).Include(b => b.Bike).ToListAsync();
        }

        // GET: api/Rentals/Unpaid
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetUnpaidRentals()
        {
            return null;
        }

        // GET: api/Rentals/$id
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }
            return rental;
        }

        // PUT: api/Rentals/$id
        [HttpPut("{id}")]
        public async Task<IActionResult> EndRental(int id, Rental rental)
        {
            if (id != rental.RentalId)
            {
                return BadRequest();
            }
            _context.Entry(rental).State = EntityState.Modified;
            try
            {
                rental.RentalEnd = DateTime.Now;
                rental.TotalCost = CalcTotalCost(rental);
                await _context.SaveChangesAsync();
                return rental;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
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

        private decimal CalcTotalCost(Rental rental)
        {
            int duration = rental.RentalEnd.Millisecond - rental.RentalBegin.Millisecond;
            decimal totalCost = 0;
            if (duration <= 15 * 60 * 1000)
            {
                return totalCost;
            }
            totalCost += rental.Bike.PriceFirstHour;
            for (duration -= 60 * 60 * 1000; duration > 0; duration -= 60 * 60 * 1000)
            {
                totalCost += rental.Bike.PriceAdditionalHours;
            }
            return totalCost;
        }

        // POST: api/Rentals
        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            if (rental.Customer.Rentals.Last().RentalEnd == null && rental.Customer.Rentals.Last().TotalCost <= 0)
            {
                throw new NotImplementedException();
            }
            if (rental.TotalCost != 0 || rental.RentalEnd != null)
            {
                throw new NotImplementedException();
            }
            rental.RentalBegin = DateTime.Now;
            rental.TotalCost = -1;
            _context.Rentals.Add(rental);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalId }, rental);
        }

        // Put: api/Rental/$id/Pay
        [HttpPut]
        [Route("{id}/Pay")]
        public async Task<IActionResult> PayRental(int id, Rental rental)
        {
            if (id != rental.RentalId)
            {
                return BadRequest();
            }

            _context.Entry(rental).State = EntityState.Modified;

            try
            {
                if (rental.TotalCost > 0)
                {
                    rental.Paid = true;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
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

        // DELETE: api/Rentals/$id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rental>> DeleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
                await _context.SaveChangesAsync();

                return rental;
            }
            return NotFound();
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.RentalId == id);
        }
    }
}
