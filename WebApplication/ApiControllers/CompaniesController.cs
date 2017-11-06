using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Authorization;
using PackageDelivery.WebApplication.Base;
using PackageDelivery.WebApplication.Data;
using PackageDelivery.WebApplication.Filters;
using PackageDelivery.WebApplication.Models;
using PackageDelivery.WebApplication.Models.Api;

namespace PackageDelivery.WebApplication.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Companies")]
    [Authorize]
    [ValidateModel]
    public class CompaniesController : Controller
    {
        private readonly PackageDeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CompaniesController(PackageDeliveryContext context, IMapper mapper, UserManager<User> userManager) {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: api/Companies
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCompanies() {
            return Ok(_mapper.Map<IEnumerable<CompanyModel>>(_context.Companies));
        }

        // GET: api/Companies/5
        [HttpGet("{companyId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompany([FromRoute] int id)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(m => m.CompanyId == id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CompanyModel>(company));
        }

        // PUT: api/Companies/5
        [HttpPut("{companyId}")]
        [Authorize(Policy = Policy.CompanyMember)]
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> PutCompany([FromRoute] int companyId, [FromBody] CompanyModel companyModel) {
            if (companyId != companyModel.CompanyId)
            {
                return BadRequest();
            }

            var company = _mapper.Map<Company>(companyModel);

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(companyId))
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

        // POST: api/Companies
        [HttpPost]
        public async Task<IActionResult> PostCompany([FromBody] CompanyModel companyModel) {
            var user = await _userManager.FindByIdAsync(User.Claims.Single(o => o.Type == Claims.NameIdentifier).Value);

            if (user.CompanyId.HasValue) {
                return BadRequest("User cannot create more than one company.");
            }

            var company = _mapper.Map<Company>(companyModel);

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            
            user.CompanyId = company.CompanyId;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await _userManager.AddClaimAsync(user, new Claim(Claims.Role, UserRoles.ADMIN));
            await _userManager.AddClaimAsync(user, new Claim(Claims.CompanyId, company.CompanyId.ToString()));

            companyModel.CompanyId = company.CompanyId;
            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, companyModel);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{companyId}")]
        [Authorize(Policy = Policy.CompanyMember)]
        [Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> DeleteCompany([FromRoute] int companyId) {
            var company = await _context.Companies.SingleOrDefaultAsync(m => m.CompanyId == companyId);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(User.Claims.Single(o => o.Type == Claims.NameIdentifier).Value);
            await _userManager.RemoveClaimAsync(user, User.Claims.Single(o => o.Type == Claims.CompanyId));

            return Ok(_mapper.Map<CompanyModel>(company));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}