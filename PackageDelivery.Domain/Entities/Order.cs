using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PackageDelivery.Domain.Entities {
    public class Order {
        public int OrderId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Payment Payment { get; set; }
        //public int PaymentId { get; set; }
        public OrderState OrderState { get; set; }
        public Address PickUpAddress { get; set; }
        public int PickUpAddressId { get; set; }
        public Address DeliveryAddress { get; set; }
        public int DeliveryAddressId { get; set; }
        public ICollection<Item> Items { get; set; }
        public PickUpPoint PickUpPoint { get; set; }
        public int? PickUpPointId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Shipment PickUpShipment { get; set; }
        public int? PickUpShipmentId { get; set; }
        public Shipment DeliveryShipment { get; set; }
        public int? DeliveryShipmentId { get; set; }
        public double DistanceBetweenPickUpAndDeliveryAddresses { get; set; }
        public double DistanceBetweenPickUpPointAndDeliveryAddress { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public void SetOrderMeasurements() {
            Weight = Items.Sum(o => o.Weight);
            Volume = Items.Sum(o => o.Volume);
            Length = Items.Sum(o => o.Length);
            Width = Items.Sum(o => o.Width);
            Height = Items.Sum(o => o.Height);
        }
    }
}
