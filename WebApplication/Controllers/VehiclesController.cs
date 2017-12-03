using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Data;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly PackageDeliveryContext _context;
        private readonly IAuthorizationService _authorizationService;

        public VehiclesController(PackageDeliveryContext context, IAuthorizationService authorizationService) {
            _context = context;
            _authorizationService = authorizationService;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var packageDeliveryContext = _context.Vehicles.Include(v => v.VehicleModel);
            return View(await packageDeliveryContext.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleModel)
                .SingleOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["VehicleModelId"] = new SelectList(_context.VehicleModels, "VehicleModelId", "VehicleModelId");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,IsActive,PlateNumber,Color,Weight,TrunkVolume,TrunkWidth,TrunkLength,DateOfManufacture,CarRegistrationDate,VehicleState,TechnicalReviewDate,VehicleModelId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["VehicleModelId"] = new SelectList(_context.VehicleModels, "VehicleModelId", "VehicleModelId", vehicle.VehicleModelId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["VehicleModelId"] = new SelectList(_context.VehicleModels, "VehicleModelId", "VehicleModelId", vehicle.VehicleModelId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleId,IsActive,PlateNumber,Color,Weight,TrunkVolume,TrunkWidth,TrunkLength,DateOfManufacture,CarRegistrationDate,VehicleState,TechnicalReviewDate,VehicleModelId")] Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.VehicleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["VehicleModelId"] = new SelectList(_context.VehicleModels, "VehicleModelId", "VehicleModelId", vehicle.VehicleModelId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleModel)
                .SingleOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.VehicleId == id);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }
}
