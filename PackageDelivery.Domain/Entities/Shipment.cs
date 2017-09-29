using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageDelivery.Domain.Entities {
    public class Shipment {
        public int ShipmentId { get; set; }
        public ShipmentState ShipmentState { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime FinishDate { get; set; }
        [InverseProperty("PickUpShipment")]
        public ICollection<Order> PickUpOrders { get; set; }
        [InverseProperty("DeliveryShipment")]
        public ICollection<Order> DeliveryOrders { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }

        public bool IsPickUp() {
            return ShipmentState == ShipmentState.PickupDone ||
                   ShipmentState == ShipmentState.InPickup ||
                   ShipmentState == ShipmentState.ReadyToStartPickup ||
                   ShipmentState == ShipmentState.WaitingForManagerApprovalToStartPickUp;
        }
    }
}
