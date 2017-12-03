using System;

namespace PackageDelivery.Domain.Dtos.VehicleDtos {
    public class VehicleUpdateDto {
        public string PlateNumber { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
        public double TrunkVolume { get; set; }
        public double TrunkHeight { get; set; }
        public double TrunkWidth { get; set; }
        public double TrunkLength { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public DateTime TechnicalReviewDate { get; set; }
        public int VehicleModelId { get; set; }
        public int PickUpPointId { get; set; }
    }
}
