using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Data;

namespace PackageDelivery.WebApplication.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Companies/{companyId}/Vehicles")]
    public class VehiclesController : Controller
    {
        private readonly PackageDeliveryContext _context;

        public VehiclesController(PackageDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public IEnumerable<Vehicle> GetVehicles([FromRoute] int companyId)
        {
            return _context.Vehicles;
        }

        // GET: api/Vehicles/5
        [HttpGet("{vehicleId}")]
        public async Task<IActionResult> GetVehicle([FromRoute] int companyId, [FromRoute] int vehicleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.VehicleId == vehicleId);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        // PUT: api/Vehicles/5
        [HttpPut("{vehicleId}")]
        public async Task<IActionResult> PutVehicle([FromRoute] int companyId, [FromRoute] int vehicleId, [FromBody] Vehicle vehicle) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vehicleId != vehicle.VehicleId)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(vehicleId))
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

        // POST: api/Vehicles
        [HttpPost]
        public async Task<IActionResult> PostVehicle([FromRoute] int companyId, [FromBody] Vehicle vehicle) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.VehicleId }, vehicle);
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] int companyId, [FromRoute] int vehicleId) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.VehicleId == vehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return Ok(vehicle);
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }
}