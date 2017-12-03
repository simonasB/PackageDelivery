using System;
using System.Collections.Generic;

namespace PackageDelivery.Domain.Entities {
    public class Country {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
