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
    [Route("api/Companies/{companyId}/PickUpPoints")]
    public class PickUpPointsController : Controller
    {
        private readonly PackageDeliveryContext _context;

        public PickUpPointsController(PackageDeliveryContext context)
        {
            _context = context;
        }

        // GET: api/PickUpPoints
        [HttpGet]
        public IEnumerable<PickUpPoint> GetPickUpPoints([FromRoute] int companyId)
        {
            return _context.PickUpPoints;
        }

        // GET: api/PickUpPoints/5
        [HttpGet("{pickUpPointId}")]
        public async Task<IActionResult> GetPickUpPoint([FromRoute] int pickUpPointId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pickUpPoint = await _context.PickUpPoints.SingleOrDefaultAsync(m => m.PickUpPointId == pickUpPointId);

            if (pickUpPoint == null)
            {
                return NotFound();
            }

            return Ok(pickUpPoint);
        }

        // PUT: api/PickUpPoints/5
        [HttpPut("{pickUpPointId}")]
        public async Task<IActionResult> PutPickUpPoint([FromRoute] int companyId, [FromRoute] int pickUpPointId, [FromBody] PickUpPoint pickUpPoint) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pickUpPointId != pickUpPoint.PickUpPointId)
            {
                return BadRequest();
            }

            _context.Entry(pickUpPoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PickUpPointExists(pickUpPointId))
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

        // POST: api/PickUpPoints
        [HttpPost]
        public async Task<IActionResult> PostPickUpPoint([FromRoute] int companyId, [FromBody] PickUpPoint pickUpPoint) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PickUpPoints.Add(pickUpPoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPickUpPoint", new { id = pickUpPoint.PickUpPointId }, pickUpPoint);
        }

        // DELETE: api/PickUpPoints/5
        [HttpDelete("{pickUpPointId}")]
        public async Task<IActionResult> DeletePickUpPoint([FromRoute] int companyId, [FromRoute] int pickUpPointId) {
            return Ok();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pickUpPoint = await _context.PickUpPoints.SingleOrDefaultAsync(m => m.PickUpPointId == pickUpPointId);
            if (pickUpPoint == null)
            {
                return NotFound();
            }

            _context.PickUpPoints.Remove(pickUpPoint);
            await _context.SaveChangesAsync();

            return Ok(pickUpPoint);
        }

        private bool PickUpPointExists(int id)
        {
            return _context.PickUpPoints.Any(e => e.PickUpPointId == id);
        }
    }
}