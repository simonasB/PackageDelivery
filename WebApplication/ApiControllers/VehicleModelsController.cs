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
    [Route("api/VehicleMakes/{vehicleMakeId}/VehicleModels")]
    public class VehicleModelsController : Controller
    {
        private readonly PackageDeliveryContext _context;

        public VehicleModelsController(PackageDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/VehicleModels
        [HttpGet]
        public IEnumerable<VehicleModel> GetVehicleModels([FromRoute] int vehicleMakeId)
        {
            return _context.VehicleModels;
        }

        // GET: api/VehicleModels/5
        [HttpGet("{vehicleModelId}")]
        public async Task<IActionResult> GetVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleModel = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<VehicleModel>(_context.VehicleModels, m => m.VehicleModelId == vehicleModelId);

            if (vehicleModel == null)
            {
                return NotFound();
            }

            return Ok(vehicleModel);
        }

        // PUT: api/VehicleModels/5
        [HttpPut("{vehicleModelId}")]
        public async Task<IActionResult> PutVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId, [FromBody] VehicleModel vehicleModel)
        {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vehicleModelId != vehicleModel.VehicleModelId)
            {
                return BadRequest();
            }

            _context.Entry(vehicleModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleModelExists(vehicleModelId))
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
        public async Task<IActionResult> PostVehicleModel([FromRoute] int vehicleMakeId, [FromBody] VehicleModel vehicleModel)
        {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicleModel", new { id = vehicleModel.VehicleModelId }, vehicleModel);
        }

        // DELETE: api/VehicleModels/5
        [HttpDelete("{vehicleModelId}")]
        public async Task<IActionResult> DeleteVehicleModel([FromRoute] int vehicleMakeId, [FromRoute] int vehicleModelId) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleModel = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<VehicleModel>(_context.VehicleModels, m => m.VehicleModelId == vehicleModelId);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            _context.VehicleModels.Remove(vehicleModel);
            await _context.SaveChangesAsync();

            return Ok(vehicleModel);
        }

        private bool VehicleModelExists(int id)
        {
            return Queryable.Any<VehicleModel>(_context.VehicleModels, e => e.VehicleModelId == id);
        }
    }
}