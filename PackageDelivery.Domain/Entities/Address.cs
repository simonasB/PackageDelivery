using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageDelivery.Domain.Entities {
    public class Address {
        public int AddressId { get; set; }
        public string HouseNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public DateTime RegistrationDate { get; set; }
        public PickUpPoint PickUpPoint { get; set; }
        //public int? PickUpPointId { get; set; }
        [InverseProperty("PickUpAddress")]        
        public ICollection<Order> PickUpOrders { get; set; }
        [InverseProperty("DeliveryAddress")]
        public ICollection<Order> DeliveryOrders { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public override string ToString() {
            return $"{StreetName.Replace(' ', '+')}+{HouseNumber},{City}";
        }
    }
}
