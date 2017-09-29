using System;
using System.Collections;
using System.Collections.Generic;

namespace PackageDelivery.Domain.Entities {
    public class Company {
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }
        public double KilometerPrice { get; set; }
        public double KilogramPrice { get; set; }
        public double CubePrice { get; set; }
        public double VolumeAndWeightRatioToApplyVolumePrice { get; set; }
        public ICollection<PickUpPoint> PickUpPoints { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
