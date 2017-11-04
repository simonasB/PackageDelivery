using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Data;

namespace PackageDelivery.WebApplication.ApiControllers
{
    [Route("api/VehicleMakes")]
    [Authorize]
    public class VehicleMakesController : Controller
    {
        private readonly PackageDeliveryContext _context;

        public VehicleMakesController(PackageDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/VehicleMakes
        [HttpGet]
        public IEnumerable<VehicleMake> GetVehicleMakes()
        {
            
            return _context.VehicleMakes;
        }

        // GET: api/VehicleMakes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleMake([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleMake = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<VehicleMake>(_context.VehicleMakes, m => m.VehicleMakeId == id);

            if (vehicleMake == null)
            {
                return NotFound();
            }

            return Ok(vehicleMake);
        }

        // PUT: api/VehicleMakes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleMake([FromRoute] int id, [FromBody] VehicleMake vehicleMake) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicleMake.VehicleMakeId)
            {
                return BadRequest();
            }

            _context.Entry(vehicleMake).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleMakeExists(id))
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

        // POST: api/VehicleMakes
        [HttpPost]
        public async Task<IActionResult> PostVehicleMake([FromBody] VehicleMake vehicleMake) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.VehicleMakes.Add(vehicleMake);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicleMake", new { id = vehicleMake.VehicleMakeId }, vehicleMake);
        }

        // DELETE: api/VehicleMakes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleMake([FromRoute] int id) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleMake = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<VehicleMake>(_context.VehicleMakes, m => m.VehicleMakeId == id);
            if (vehicleMake == null)
            {
                return NotFound();
            }

            _context.VehicleMakes.Remove(vehicleMake);
            await _context.SaveChangesAsync();

            return Ok(vehicleMake);
        }

        private bool VehicleMakeExists(int id)
        {
            return Queryable.Any<VehicleMake>(_context.VehicleMakes, e => e.VehicleMakeId == id);
        }
    }
}