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
    public class CustomersController : ControllerBase
    {
        private readonly BikeRentalDbContext _context;

        public CustomersController(BikeRentalDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        [Route("{lastNameFilter?}")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(string lastNameFilter = null)
        {
            if (lastNameFilter != null)
            {
                return await _context.Customers.Where(c => c.LastName == lastNameFilter).ToListAsync();
            }
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/$id
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/$id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/$id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        // GET: api/Customers/$id/rentals
        [HttpGet("{id}/rentals")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetCustomerRentals(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer.Rentals.ToList();
        }

        // Returns true, if a customer with the $id exists
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
