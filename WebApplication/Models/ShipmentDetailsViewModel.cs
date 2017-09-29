using System.Collections.Generic;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Models
{
    public class ShipmentDetailsViewModel
    {
        public Shipment Shipment { get; set; }
        public string DirectionsRequest { get; set; }
    }
}
