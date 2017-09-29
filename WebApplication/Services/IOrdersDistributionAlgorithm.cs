using System.Collections.Generic;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Services
{
    public interface IOrdersDistributionAlgorithm {
        List<Shipment> Distribute(List<User> drivers, List<Vehicle> vehicles, List<Order> orders, ShipmentState shipmentState);
    }
}
