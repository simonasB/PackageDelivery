using System;
using System.Collections.Generic;
using System.Linq;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Services
{
    public class OptimalOrderDistributionAlgorithm : IOrdersDistributionAlgorithm
    {
        private readonly IRouteCalculator _routeCalculator;

        public OptimalOrderDistributionAlgorithm(IRouteCalculator routeCalculator) {
            _routeCalculator = routeCalculator;
        }

        public List<Shipment> Distribute(List<User> drivers, List<Vehicle> vehicles, List<Order> orders, ShipmentState shipmentState) {
            var availableDriversAndVehiclesCount = vehicles.Count >= drivers.Count ? drivers.Count : vehicles.Count;

            orders.Sort((y, x) => x.Items.Sum(i => i.Volume).CompareTo(y.Items.Sum(i => i.Volume)));
            vehicles.Sort((y, x) => x.TrunkVolume.CompareTo(y.TrunkVolume));

            var newShipments = new List<Shipment>();

            for (int i = 0; i < availableDriversAndVehiclesCount; i++)
            {
                var vehicle = vehicles[i];

                var shipmentOrders = new List<Order>();

                var shipment = new Shipment
                {
                    CreationDate = DateTime.UtcNow,
                    UserId = drivers[i].Id,
                    VehicleId = vehicle.VehicleId,
                    Vehicle = vehicle
                };

                double totalShipmentVolume = 0;
                double totalShipmentWeight = 0;

                var ordersCountBeforeShipping = orders.Count;

                int currentOrderIndex = 0;

                for (int j = 0; j < ordersCountBeforeShipping; j++)
                {
                    var order = orders[currentOrderIndex];

                    if (order != null)
                    {
                        if ((totalShipmentVolume + order.Volume) > vehicle.TrunkVolume ||
                            (totalShipmentWeight + order.Weight) > vehicle.MaxTrunkWeight
                        )
                        {
                            currentOrderIndex++;
                            continue;
                        }

                        shipmentOrders.Add(order);
                        orders.Remove(order);

                        totalShipmentWeight += order.Weight;
                        totalShipmentVolume += order.Volume;
                    }
                    else
                    {
                        break;
                    }
                }

                bool isRouteTooLong = true;

                while (isRouteTooLong)
                {
                    var routeTime = _routeCalculator.CalculateRouteTime(drivers[i].PickUpPoint.Address,
                        drivers[i].PickUpPoint.Address,
                        shipmentOrders.Select(o => shipment.ShipmentState == ShipmentState.WaitingForManagerApprovalToStartPickUp ? o.PickUpAddress : o.DeliveryAddress));

                    if (routeTime > TimeSpan.FromHours(8).TotalSeconds)
                    {
                        var orderToRemoveFromShipment = shipmentOrders.Last();
                        shipmentOrders.Remove(orderToRemoveFromShipment);
                        orders.Add(orderToRemoveFromShipment);
                    }
                    else
                    {
                        isRouteTooLong = false;
                    }
                }

                shipment.Vehicle.VehicleState = VehicleState.OnShipment;

                if (shipmentState == ShipmentState.WaitingForManagerApprovalToStartPickUp) {
                    shipment.PickUpOrders = shipmentOrders;
                } else if (shipmentState == ShipmentState.WaitingForManagerApprovalToStartDelivery) {
                    shipment.DeliveryOrders = shipmentOrders;
                }

                newShipments.Add(shipment);
                if (orders.Count == 0)
                {
                    break;
                }

                orders.Sort((y, x) => x.Items.Sum(it => it.Volume).CompareTo(y.Items.Sum(it => it.Volume)));
            }

            return newShipments;
        }
    }
}
