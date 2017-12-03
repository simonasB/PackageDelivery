using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PackageDelivery.Core;
using PackageDelivery.Data;
using PackageDelivery.Domain.Entities;
using PackageDelivery.Services.Services;
using PackageDelivery.WebApplication.Models;

namespace PackageDelivery.WebApplication.Controllers
{
    public class PickUpShipmentsController : Controller
    {
        private readonly PackageDeliveryContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IShipmentManagementProvider _shipmentManagementProvider;

        public PickUpShipmentsController(IShipmentManagementProvider shipmentManagementProvider, PackageDeliveryContext context, UserManager<User> userManager) {
            _shipmentManagementProvider = shipmentManagementProvider;
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = UserRoles.MANAGER)]
        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var packageDeliveryContext = _context.Shipments
                .Where(o => o.Vehicle.PickUpPointId == user.PickUpPointId && 
                (o.ShipmentState == ShipmentState.PickupDone ||
                o.ShipmentState == ShipmentState.InPickup ||
                o.ShipmentState == ShipmentState.ReadyToStartPickup ||
                o.ShipmentState == ShipmentState.WaitingForManagerApprovalToStartPickUp))
                .Include(s => s.User)
                .Include(s => s.PickUpOrders)
                .Include(s => s.Vehicle);

            return View("~/Views/Shipments/Index.cshtml", await packageDeliveryContext.ToListAsync());
        }

        [Authorize(Roles = UserRoles.MANAGER + "," + UserRoles.DRIVER)]
        // GET: Shipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.User)
                .Include(s => s.Vehicle)
                .ThenInclude(o => o.PickUpPoint.Address)
                .Include(s => s.PickUpOrders)
                .ThenInclude(o => o.PickUpAddress)
                .Include(s => s.PickUpOrders)
                .ThenInclude(o => o.DeliveryAddress)
                .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }

            var shipmentDetailsViewModel = new ShipmentDetailsViewModel {
                Shipment = shipment,
                DirectionsRequest = JsonConvert.SerializeObject(new DirectionsRequest {
                    Origin = shipment.Vehicle.PickUpPoint.Address.ToString(),
                    Destination = shipment.Vehicle.PickUpPoint.Address.ToString(),
                    WayPoints = shipment.PickUpOrders
                        .Select(o => new WayPoint { Location = o.PickUpAddress.ToString(), StopOver = true }).ToList(),
                    TravelMode = "DRIVING"
                })
            };

            return View("~/Views/Shipments/Details.cshtml", shipmentDetailsViewModel);
        }

        [Authorize(Roles = UserRoles.MANAGER)]
        // GET: Shipments/Details/5
        public async Task<IActionResult> Approve(int? id) {
            if (id == null) {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null) {
                return NotFound();
            }

            shipment.ShipmentState = ShipmentState.ReadyToStartPickup;
            _context.Shipments.Update(shipment);

            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", new {id = shipment.ShipmentId});
        }

        [Authorize(Roles = UserRoles.MANAGER)]
        // GET: Shipments/Details/5
        public async Task<IActionResult> Cancel(int? id) {
            if (id == null) {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null) {
                return NotFound();
            }

            shipment.ShipmentState = ShipmentState.WaitingForManagerApprovalToStartPickUp;
            _context.Shipments.Update(shipment);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new {id = shipment.ShipmentId});
        }

        [Authorize(Roles = UserRoles.MANAGER)]
        // GET: Shipments/Create
        public async Task<IActionResult> Create()
        {
            var manager = await _userManager.GetUserAsync(HttpContext.User);

            var error = _shipmentManagementProvider.CreatePossibleShipments(
                ShipmentState.WaitingForManagerApprovalToStartPickUp, manager);

            TempData["InfoMessage"] = error.GetResult();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = UserRoles.MANAGER)]
        // GET: Shipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.User)
                .Include(s => s.Vehicle)
                .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View("~/Views/Shipments/Delete.cshtml", shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.MANAGER)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shipment = await _context.Shipments
                .Include(s => s.User)
                .Include(s => s.Vehicle)
                .Include(s => s.PickUpOrders)
                .SingleOrDefaultAsync(m => m.ShipmentId == id);

            shipment.User = null;

            shipment.Vehicle.VehicleState = VehicleState.AtParkingLot;
            shipment.Vehicle = null;

            foreach (var shipmentPickUpOrder in shipment.PickUpOrders) {
                shipmentPickUpOrder.OrderState = OrderState.ReadyToBePickedUp;
            }
            shipment.PickUpOrders = null;

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
