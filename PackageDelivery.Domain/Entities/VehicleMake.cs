using System.Collections.Generic;

namespace PackageDelivery.Domain.Entities {
    public class VehicleMake {
        public int VehicleMakeId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public ICollection<VehicleModel> VehicleModels { get; set; }
    }
}
