using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Data;
using PackageDelivery.Domain.Dtos.VehicleModelDtos;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Authorization;
using PackageDelivery.WebApplication.Filters;

namespace PackageDelivery.WebApplication.ApiControllers
{
    [Produces("application/json")]
    [Route("api/VehicleMakes/{vehicleMakeId}/VehicleModels")]
    [ValidateModel]
    public class VehicleModelsController : Controller
    {
        private readonly PackageDeliveryContext _context;
        private readonly IMapper _mapper;

        public VehicleModelsController(PackageDeliveryContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/VehicleModels
        [HttpGet]
        [Authorize]
        public IActionResult GetVehicleModels([FromRoute] int vehicleMakeId) {
            return Ok(_mapper.Map<IEnumerable<VehicleModelDto>>(_context.VehicleModels
                .Where(o => o.VehicleMakeId == vehicleMakeId).Include(o => o.VehicleMake)));
        }

        // GET: api/VehicleModels/5
        [HttpGet("{vehicleModelId}")]
        [Authorize]
        public async Task<IActionResult> GetVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId) {
            var vehicleMake = await _context.VehicleMakes.SingleOrDefaultAsync(o => o.VehicleMakeId == vehicleMakeId);
            if (vehicleMake == null) {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels.SingleOrDefaultAsync(o => o.VehicleModelId == vehicleModelId);
            if (vehicleModel == null) {
                return NotFound();
            }

            return Ok(_mapper.Map<VehicleModelDto>(vehicleModel));
        }

        // PUT: api/VehicleModels/5
        [HttpPut("{vehicleModelId}")]
        [Authorize(Policy = Policy.SuperAdmin)]
        public async Task<IActionResult> PutVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId, [FromBody] VehicleModelUpdateDto vehicleModelDto)
        {
            var vehicleMake = await _context.VehicleMakes.SingleOrDefaultAsync(o => o.VehicleMakeId == vehicleMakeId);
            if (vehicleMake == null) {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels.SingleOrDefaultAsync(o => o.VehicleModelId == vehicleModelId);
            if (vehicleModel == null) {
                return NotFound();
            }

            _mapper.Map(vehicleModelDto, vehicleModel);

            _context.Entry(vehicleModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleModelExists(vehicleModelId, vehicleMakeId))
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

        // POST: api/VehicleModels
        [HttpPost]
        [Authorize(Policy = Policy.SuperAdmin)]
        public async Task<IActionResult> PostVehicleModel([FromRoute] int vehicleMakeId, [FromBody] VehicleModelCreationDto vehicleModelDto) {
            var vehicleMake = await _context.VehicleMakes.SingleOrDefaultAsync(o => o.VehicleMakeId == vehicleMakeId);

            if (vehicleMake == null) {
                return NotFound();
            }

            var vehicleModel = _mapper.Map<VehicleModel>(vehicleModelDto);
            vehicleModel.VehicleMakeId = vehicleMakeId;

            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicleModel", new { id = vehicleModel.VehicleModelId }, _mapper.Map<VehicleModelDto>(vehicleModel));
        }

        // DELETE: api/VehicleModels/5
        [HttpDelete("{vehicleModelId}")]
        [Authorize(Policy = Policy.SuperAdmin)]
        public async Task<IActionResult> DeleteVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId) {
            var vehicleMake = await _context.VehicleMakes.SingleOrDefaultAsync(o => o.VehicleMakeId == vehicleMakeId);
            if (vehicleMake == null) {
                return NotFound();
            }

            var vehicleModel = await _context.VehicleModels.SingleOrDefaultAsync(o => o.VehicleModelId == vehicleModelId);
            if (vehicleModel == null) {
                return NotFound();
            }

            _context.VehicleModels.Remove(vehicleModel);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<VehicleModelDto>(vehicleModel));
        }

        private bool VehicleModelExists(int vehicleModelId, int vehicleMakeId)
        {
            return _context.VehicleModels.Include(o => o.VehicleMake).Any(m =>
                m.VehicleModelId == vehicleModelId && m.VehicleMake.VehicleMakeId == vehicleMakeId);
        }
    }
}