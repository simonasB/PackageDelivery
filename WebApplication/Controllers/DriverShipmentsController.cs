using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Base;
using PackageDelivery.WebApplication.Data;
using PackageDelivery.WebApplication.Models;

namespace PackageDelivery.WebApplication.Controllers
{
    [Authorize(Roles = UserRoles.DRIVER)]
    public class DriverShipmentsController : Controller {
        private readonly PackageDeliveryContext _context;
        private readonly UserManager<User> _userManager;

        public DriverShipmentsController(PackageDeliveryContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            var packageDeliveryContext = _context.Shipments
                .Where(o => o.UserId == user.Id &&
                o.ShipmentState != ShipmentState.WaitingForManagerApprovalToStartDelivery &&
                o.ShipmentState != ShipmentState.WaitingForManagerApprovalToStartPickUp)
                .Include(s => s.User)
                .Include(s => s.PickUpOrders)
                .Include(s => s.DeliveryOrders)
                .Include(s => s.Vehicle);

            return View("~/Views/Shipments/Index.cshtml", await packageDeliveryContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
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
                .Include(s => s.DeliveryOrders)
                .ThenInclude(o => o.PickUpAddress)
                .Include(s => s.DeliveryOrders)
                .ThenInclude(o => o.DeliveryAddress)
                .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null) {
                return NotFound();
            }

            List<WayPoint> wayPoints;

            if (shipment.IsPickUp()) {
                wayPoints = shipment.PickUpOrders
                    .Select(o => new WayPoint {Location = o.PickUpAddress.ToString(), StopOver = true}).ToList();
            } else {
                wayPoints = shipment.DeliveryOrders
                    .Select(o => new WayPoint { Location = o.DeliveryAddress.ToString(), StopOver = true }).ToList();
            }

            var shipmentDetailsViewModel = new ShipmentDetailsViewModel {
                Shipment = shipment,
                DirectionsRequest = JsonConvert.SerializeObject(new DirectionsRequest {
                    Origin = shipment.Vehicle.PickUpPoint.Address.ToString(),
                    Destination = shipment.Vehicle.PickUpPoint.Address.ToString(),
                    WayPoints = wayPoints,
                    TravelMode = "DRIVING"
                })
            };

            return View("~/Views/Shipments/Details.cshtml", shipmentDetailsViewModel);
        }

        public async Task<IActionResult> Start(int? id) {
            if (id == null) {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(o => o.Vehicle)
                .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null) {
                return NotFound();
            }

            if (shipment.ShipmentState == ShipmentState.ReadyToStartDelivery) {
                shipment.ShipmentState = ShipmentState.InDelivery;
            }

            if (shipment.ShipmentState == ShipmentState.ReadyToStartPickup) {
                shipment.ShipmentState = ShipmentState.InPickup;
            }

            shipment.Vehicle.VehicleState = VehicleState.OnShipment;

            _context.Shipments.Update(shipment);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new {id = shipment.ShipmentId});
        }

        public async Task<IActionResult> End(int? id) {
            if (id == null) {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(o => o.PickUpOrders)
                .Include(o => o.DeliveryOrders)
                .Include(o => o.Vehicle)
                .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null) {
                return NotFound();
            }

            if (shipment.ShipmentState == ShipmentState.InDelivery) {
                shipment.ShipmentState = ShipmentState.DeliveryDone;

                foreach (var shipmentDeliveryOrder in shipment.DeliveryOrders) {
                    shipmentDeliveryOrder.OrderState = OrderState.Delivered;
                }
            }

            if (shipment.ShipmentState == ShipmentState.InPickup) {
                shipment.ShipmentState = ShipmentState.PickupDone;

                foreach (var shipmentPickUpOrder in shipment.PickUpOrders) {
                    shipmentPickUpOrder.OrderState = OrderState.ReadyToBeDelivered;
                }
            }

            shipment.Vehicle.VehicleState = VehicleState.AtParkingLot;

            shipment.FinishDate = DateTime.UtcNow;

            _context.Shipments.Update(shipment);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new {id = shipment.ShipmentId});
        }
    }
}
