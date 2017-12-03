using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Data;
using PackageDelivery.Domain.Dtos.VehicleDtos;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Authorization;
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
        private readonly IMapper _mapper;

        public VehiclesController(PackageDeliveryContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Vehicles
        [HttpGet]     
        public IActionResult GetVehicles([FromRoute] int companyId) {
            return Ok(_mapper.Map<IEnumerable<VehicleDto>>(_context.Vehicles.Where(o => o.CompanyId == companyId)
                .Include(o => o.Company).Include(o => o.PickUpPoint).Include(o => o.VehicleModel)));
        }

        // GET: api/Vehicles/5
        [HttpGet("{vehicleId}")]
        public async Task<IActionResult> GetVehicle([FromRoute] int companyId, [FromRoute] int vehicleId)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(o => o.CompanyId == companyId);
            if (company == null) {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.Include(o => o.Company).Include(o => o.PickUpPoint).Include(o => o.VehicleModel).SingleOrDefaultAsync(m => m.VehicleId == vehicleId);

            if (vehicle == null) {
                return NotFound();
            }

            return Ok(_mapper.Map<VehicleDto>(vehicle));
        }

        // PUT: api/Vehicles/5
        [HttpPut("{vehicleId}")]
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> PutVehicle([FromRoute] int companyId, [FromRoute] int vehicleId, [FromBody] VehicleUpdateDto vehicleDto) {
            var company = await _context.Companies.SingleOrDefaultAsync(o => o.CompanyId == companyId);
            if (company == null) {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.Include(o => o.Company).Include(o => o.PickUpPoint).Include(o => o.VehicleModel).SingleOrDefaultAsync(m => m.VehicleId == vehicleId);

            if (vehicle == null) {
                return NotFound();
            }

            _mapper.Map(vehicleDto, vehicle);

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(companyId, vehicleId))
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
        public async Task<IActionResult> PostVehicle([FromRoute] int companyId, [FromBody] VehicleCreationDto vehicleDto) {
            var company = await _context.Companies.SingleOrDefaultAsync(o => o.CompanyId == companyId);
            if (company == null) {
                return NotFound();
            }

            var pickUpPoint =
                await _context.PickUpPoints.SingleOrDefaultAsync(o => o.PickUpPointId == vehicleDto.PickUpPointId);

            if (pickUpPoint == null || pickUpPoint.CompanyId != companyId) {
                return NotFound();
            }

            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            vehicle.CompanyId = companyId;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.VehicleId }, _mapper.Map<VehicleDto>(vehicle));
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("{vehicleId}")]
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> DeleteVehicle([FromRoute] int companyId, [FromRoute] int vehicleId) {
            var company = await _context.Companies.SingleOrDefaultAsync(o => o.CompanyId == companyId);
            if (company == null) {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.VehicleId == vehicleId);
            if (vehicle == null) {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<VehicleDto>(vehicle));
        }

        private bool VehicleExists(int companyId, int vehicleId)
        {
            return _context.Vehicles.Any(e => e.CompanyId == companyId && e.VehicleId == vehicleId);
        }
    }
}