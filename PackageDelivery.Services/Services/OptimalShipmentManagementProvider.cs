using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Core;
using PackageDelivery.Data;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.Services.Services
{
    public class OptimalShipmentManagementProvider : IShipmentManagementProvider
    {
        private readonly PackageDeliveryContext _context;
        private readonly IOrdersDistributionAlgorithm _ordersDistributionAlgorithm;
        private readonly UserManager<User> _userManager;

        public OptimalShipmentManagementProvider(IOrdersDistributionAlgorithm ordersDistributionAlgorithm, PackageDeliveryContext context, UserManager<User> userManager) {
            _ordersDistributionAlgorithm = ordersDistributionAlgorithm;
            _context = context;
            _userManager = userManager;
        }

        public IShipmentsCreationError CreatePossibleShipments(ShipmentState shipmentState, User manager) {
            var orderState = shipmentState == ShipmentState.WaitingForManagerApprovalToStartPickUp
                ? OrderState.ReadyToBePickedUp
                : OrderState.ReadyToBeDelivered;

            var orders = _context.Orders.Where(
                    o => o.PickUpPoint.PickUpPointId == manager.PickUpPointId &&
                         o.Payment.PaymentState == PaymentState.Paid &&
                         o.OrderState == orderState). // WaitingForDelivery
                Include(o => o.PickUpPoint).
                Include(o => o.DeliveryAddress).
                Include(o => o.PickUpAddress).
                Include(o => o.Items).
                Include(o => o.Payment).ToList();

            if (!orders.Any()) {
                return new NoOrdersAvailableError();
            }

            var drivers = GetAvailableDrivers(manager);

            if (!drivers.Any())
            {
                return new NoDriversAvailableError();
            }

            var vehicles = _context.Vehicles.Where(o => o.PickUpPointId == manager.PickUpPointId && o.VehicleState == VehicleState.AtParkingLot).ToList();

            if (!vehicles.Any())
            {
                return new NoVehiclesAvailableError();
            }

            var newShipments = _ordersDistributionAlgorithm.Distribute(drivers, vehicles, orders, shipmentState);

            UpdateShipmentsAndOrdersStates(newShipments, shipmentState);

            _context.AddRange(newShipments);

            _context.SaveChanges();

            return new NullError();
        }

        private void UpdateShipmentsAndOrdersStates(List<Shipment> shipments, ShipmentState shipmentState) {
            shipments.ForEach((shipment) => {
                shipment.ShipmentState = shipmentState;
                if (shipmentState == ShipmentState.WaitingForManagerApprovalToStartPickUp) {
                    foreach (var shipmentPickUpOrder in shipment.PickUpOrders) {
                        shipmentPickUpOrder.OrderState = OrderState.OnPickingUp;
                    }
                } else {
                    foreach (var shipmentPickUpOrder in shipment.DeliveryOrders) {
                        shipmentPickUpOrder.OrderState = OrderState.OnDelivery;
                    }
                }
            });
        }

        private List<User> GetAvailableDrivers(User manager) {
            var shipments = _context.Shipments
                .Where(o => o.ShipmentState != ShipmentState.PickupDone && o.ShipmentState != ShipmentState.DeliveryDone && o.Vehicle.PickUpPointId == manager.PickUpPointId)
                .Include(o => o.Vehicle)
                .Select(o => o.UserId).ToList();

            var usersRelatedToPickUpPoint = _context.Users
                .Where(o => o.PickUpPointId == manager.PickUpPointId && !shipments.Contains(o.Id))
                .Include(o => o.PickUpPoint.Address)
                .ToList();

            var drivers = new List<User>();

            foreach (var user in usersRelatedToPickUpPoint)
            {
                var isDriver = _userManager.IsInRoleAsync(user, UserRoles.DRIVER).Result;
                if (isDriver)
                {
                    drivers.Add(user);
                }
            }

            return drivers;
        }
    }
}
