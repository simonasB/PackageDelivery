using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PackageDelivery.Domain.Entities {
    public class User : IdentityUser {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Company Company { get; set; }
        public int? CompanyId { get; set; }
        public PickUpPoint PickUpPoint  { get; set; }
        public int? PickUpPointId  { get; set; }
    }
}
