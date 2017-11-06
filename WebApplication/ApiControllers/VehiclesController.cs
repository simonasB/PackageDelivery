using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Authorization;
using PackageDelivery.WebApplication.Base;
using PackageDelivery.WebApplication.Data;
using PackageDelivery.WebApplication.Filters;

namespace PackageDelivery.WebApplication.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Companies/{companyId}/Vehicles")]
    [Authorize(Policy = Policy.CompanyMember)]
    [ValidateModel]
    public class VehiclesController : Controller
    {
        private readonly PackageDeliveryContext _context;
        private readonly IAuthorizationService _authorizationService;

        public VehiclesController(PackageDeliveryContext context, IAuthorizationService authorizationService) {
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: api/Vehicles
        [HttpGet]     
        public IEnumerable<Vehicle> GetVehicles([FromRoute] int companyId) {
            return _context.Vehicles.Where(o => o.CompanyId == companyId);
        }

        // GET: api/Vehicles/5
        [HttpGet("{vehicleId}")]
        public async Task<IActionResult> GetVehicle([FromRoute] int companyId, [FromRoute] int vehicleId)
        {
            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.VehicleId == vehicleId);

            if (vehicle == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, vehicle, Operations.Read);

            if (!isAuthorized) {
                return Unauthorized();
            }

            return Ok(vehicle);
        }

        // PUT: api/Vehicles/5
        [HttpPut("{vehicleId}")]
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> PutVehicle([FromRoute] int companyId, [FromRoute] int vehicleId, [FromBody] Vehicle vehicle) {
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
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> PostVehicle([FromRoute] int companyId, [FromBody] Vehicle vehicle) {
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
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> DeleteVehicle([FromRoute] int companyId, [FromRoute] int vehicleId) {
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