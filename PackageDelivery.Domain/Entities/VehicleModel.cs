using System.Collections.Generic;

namespace PackageDelivery.Domain.Entities {
    public class VehicleModel {
        public int VehicleModelId { get; set; }
        public string Name { get; set; }
        public VehicleMake VehicleMake { get; set; }
        public int VehicleMakeId { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}