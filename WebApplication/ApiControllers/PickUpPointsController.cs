using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Data;
using PackageDelivery.Domain.Dtos.PickUpPointDtos;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Authorization;
using PackageDelivery.WebApplication.Filters;

namespace PackageDelivery.WebApplication.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Companies/{companyId}/PickUpPoints")]
    [Authorize(Policy = Policy.CompanyMember)]
    [ValidateModel]
    public class PickUpPointsController : Controller
    {
        private readonly PackageDeliveryContext _context;
        private readonly IMapper _mapper;

        public PickUpPointsController(PackageDeliveryContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/PickUpPoints
        [HttpGet]
        public IActionResult GetPickUpPoints([FromRoute] int companyId) {
            return Ok(_mapper.Map<IEnumerable<PickUpPointDto>>(
                _context.PickUpPoints.Where(o => o.CompanyId == companyId).Include(o => o.Company)));
        }

        // GET: api/PickUpPoints/5
        [HttpGet("{pickUpPointId}")]
        public async Task<IActionResult> GetPickUpPoint([FromRoute] int companyId, [FromRoute] int pickUpPointId) {
            var company = await _context.Companies.SingleOrDefaultAsync(o => o.CompanyId == companyId);
            if (company == null) {
                return NotFound();
            }

            var pickUpPoint = await _context.PickUpPoints.Include(o => o.Company).SingleOrDefaultAsync(m => m.PickUpPointId == pickUpPointId);
            if (pickUpPoint == null) {
                return NotFound();
            }

            return Ok(_mapper.Map<PickUpPointDto>(pickUpPoint));
        }

        // PUT: api/PickUpPoints/5
        [HttpPut("{pickUpPointId}")]
        public async Task<IActionResult> PutPickUpPoint([FromRoute] int companyId, [FromRoute] int pickUpPointId, [FromBody] PickUpPointUpdateDto pickUpPointDto) {
            var company = await _context.Companies.SingleOrDefaultAsync(o => o.CompanyId == companyId);
            if (company == null) {
                return NotFound();
            }

            var pickUpPoint = await _context.PickUpPoints.Include(o => o.Company).SingleOrDefaultAsync(m => m.PickUpPointId == pickUpPointId);
            if (pickUpPoint == null) {
                return NotFound();
            }

            _mapper.Map(pickUpPointDto, pickUpPoint);

            _context.Entry(pickUpPoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PickUpPointExists(companyId, pickUpPointId))
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
        public async Task<IActionResult> PostPickUpPoint([FromRoute] int companyId, [FromBody] PickUpPointCreationDto pickUpPointDto) {
            var company = await _context.Companies.SingleOrDefaultAsync(o => o.CompanyId == companyId);
            if (company == null) {
                return NotFound();
            }

            var pickUpPoint = _mapper.Map<PickUpPoint>(pickUpPointDto);
            pickUpPoint.CompanyId = companyId;

            _context.PickUpPoints.Add(pickUpPoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPickUpPoint", new { id = pickUpPoint.PickUpPointId }, _mapper.Map<PickUpPointDto>(pickUpPoint));
        }

        // DELETE: api/PickUpPoints/5
        [HttpDelete("{pickUpPointId}")]
        public async Task<IActionResult> DeletePickUpPoint([FromRoute] int companyId, [FromRoute] int pickUpPointId) {
            var company = await _context.Companies.SingleOrDefaultAsync(o => o.CompanyId == companyId);
            if (company == null) {
                return NotFound();
            }

            var pickUpPoint = await _context.PickUpPoints.Include(o => o.Company).SingleOrDefaultAsync(m => m.PickUpPointId == pickUpPointId);
            if (pickUpPoint == null) {
                return NotFound();
            }

            _context.PickUpPoints.Remove(pickUpPoint);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<PickUpPointDto>(pickUpPoint));
        }

        private bool PickUpPointExists(int companyId, int pickUpPointId)
        {
            return _context.PickUpPoints.Any(e => e.CompanyId == companyId && e.PickUpPointId == pickUpPointId);
        }
    }
}