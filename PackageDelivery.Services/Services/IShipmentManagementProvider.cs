using PackageDelivery.Domain.Entities;

namespace PackageDelivery.Services.Services
{
    public interface IShipmentManagementProvider {
        IShipmentsCreationError CreatePossibleShipments(ShipmentState shipmentNext, User manager);
    }
}
