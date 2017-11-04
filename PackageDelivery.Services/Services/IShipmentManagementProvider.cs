using System.Collections.Generic;
using System.Threading.Tasks;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Services
{
    public interface IShipmentManagementProvider {
        IShipmentsCreationError CreatePossibleShipments(ShipmentState shipmentNext, User manager);
    }
}
