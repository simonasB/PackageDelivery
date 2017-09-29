using System;
using System.Collections;
using System.Collections.Generic;

namespace PackageDelivery.Domain.Entities {
    public class PickUpPoint {
        public int PickUpPointId { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsStatic { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
