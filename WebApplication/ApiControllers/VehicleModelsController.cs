using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Authorization;
using PackageDelivery.WebApplication.Data;
using PackageDelivery.WebApplication.Filters;
using PackageDelivery.WebApplication.Models.Api;

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
        public IActionResult GetVehicleModels([FromRoute] int vehicleMakeId)
        {
            return Ok(_mapper.Map<IEnumerable<VehicleModelViewModel>>(_context.VehicleModels.Include(o => o.VehicleMake)));
        }

        // GET: api/VehicleModels/5
        [HttpGet("{vehicleModelId}")]
        [Authorize]
        public async Task<IActionResult> GetVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId) {
            var vehicleModel = await _context.VehicleModels.Include(o => o.VehicleMake)
                .SingleOrDefaultAsync(m =>
                    m.VehicleModelId == vehicleModelId && m.VehicleMake.VehicleMakeId == vehicleMakeId);

            if (vehicleModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VehicleModelViewModel>(vehicleModel));
        }

        // PUT: api/VehicleModels/5
        [HttpPut("{vehicleModelId}")]
        [Authorize(Policy = Policy.SuperAdmin)]
        public async Task<IActionResult> PutVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId, [FromBody] VehicleModelViewModel vehicleModelViewModel)
        {
            if (vehicleMakeId != vehicleModelViewModel.VehicleMakeId) {
                return BadRequest();
            }

            if (vehicleModelId != vehicleModelViewModel.VehicleModelId) {
                return BadRequest();
            }

            var vehicleMake = await _context.VehicleMakes.SingleOrDefaultAsync(o => o.VehicleMakeId == vehicleMakeId);

            if (vehicleMake == null) {
                return NotFound();
            }

            var vehicleModel = _mapper.Map<VehicleModel>(vehicleModelViewModel);

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
        public async Task<IActionResult> PostVehicleModel([FromRoute] int vehicleMakeId, [FromBody] VehicleModelViewModel vehicleModelViewModel) {
            if (vehicleMakeId != vehicleModelViewModel.VehicleMakeId) {
                return BadRequest();
            }

            var vehicleMake = await _context.VehicleMakes.SingleOrDefaultAsync(o => o.VehicleMakeId == vehicleMakeId);

            if (vehicleMake == null) {
                return NotFound();
            }

            var vehicleModel = _mapper.Map<VehicleModel>(vehicleModelViewModel);

            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();

            vehicleModelViewModel.VehicleModelId = vehicleModel.VehicleModelId;
            return CreatedAtAction("GetVehicleModel", new { id = vehicleModelViewModel.VehicleModelId }, vehicleModelViewModel);
        }

        // DELETE: api/VehicleModels/5
        [HttpDelete("{vehicleModelId}")]
        [Authorize(Policy = Policy.SuperAdmin)]
        public async Task<IActionResult> DeleteVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId) {
            var vehicleModel = await _context.VehicleModels.Include(o => o.VehicleMake)
                .SingleOrDefaultAsync(m =>
                    m.VehicleModelId == vehicleModelId && m.VehicleMake.VehicleMakeId == vehicleMakeId);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            _context.VehicleModels.Remove(vehicleModel);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<VehicleModelViewModel>(vehicleModel));
        }

        private bool VehicleModelExists(int vehicleModelId, int vehicleMakeId)
        {
            return _context.VehicleModels.Include(o => o.VehicleMake).Any(m =>
                m.VehicleModelId == vehicleModelId && m.VehicleMake.VehicleMakeId == vehicleMakeId);
        }
    }
}