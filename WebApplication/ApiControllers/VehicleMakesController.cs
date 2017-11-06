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
using PackageDelivery.WebApplication.Models.Api.VehicleMake;

namespace PackageDelivery.WebApplication.ApiControllers
{
    [Route("api/VehicleMakes")]
    [ValidateModel]
    public class VehicleMakesController : Controller
    {
        private readonly PackageDeliveryContext _context;
        private readonly IMapper _mapper;

        public VehicleMakesController(PackageDeliveryContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/VehicleMakes
        [HttpGet]
        [Authorize]
        public IActionResult GetVehicleMakes()
        {         
            return Ok(_mapper.Map<IEnumerable<VehicleMakeModel>>(_context.VehicleMakes));
        }

        // GET: api/VehicleMakes/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetVehicleMake([FromRoute] int id)
        {
            var vehicleMake = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<VehicleMake>(_context.VehicleMakes, m => m.VehicleMakeId == id);

            if (vehicleMake == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VehicleMakeModel>(vehicleMake));
        }

        // PUT: api/VehicleMakes/5
        [HttpPut("{id}")]
        [Authorize(Policy = Policy.SuperAdmin)]
        public async Task<IActionResult> PutVehicleMake([FromRoute] int id, [FromBody] VehicleMakeModel vehicleMakeModel) {
            if (id != vehicleMakeModel.VehicleMakeId)
            {
                return BadRequest();
            }

            var vehicleMake = _mapper.Map<VehicleMake>(vehicleMakeModel);

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
        [Authorize(Policy = Policy.SuperAdmin)]
        public async Task<IActionResult> PostVehicleMake([FromBody] VehicleMakeModel vehicleMakeModel) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleMake = _mapper.Map<VehicleMake>(vehicleMakeModel);

            _context.VehicleMakes.Add(vehicleMake);
            await _context.SaveChangesAsync();

            vehicleMakeModel.VehicleMakeId = vehicleMake.VehicleMakeId;
            return CreatedAtAction("GetVehicleMake", new { id = vehicleMake.VehicleMakeId }, vehicleMakeModel);
        }

        // DELETE: api/VehicleMakes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Policy.SuperAdmin)]
        public async Task<IActionResult> DeleteVehicleMake([FromRoute] int id) {
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

            return Ok(_mapper.Map<VehicleMakeModel>(vehicleMake));
        }

        private bool VehicleMakeExists(int id)
        {
            return Queryable.Any<VehicleMake>(_context.VehicleMakes, e => e.VehicleMakeId == id);
        }
    }
}