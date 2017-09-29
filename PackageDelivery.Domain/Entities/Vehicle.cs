using System;

namespace PackageDelivery.Domain.Entities {
    public class Vehicle {
        public int VehicleId { get; set; }
        public bool IsActive { get; set; }
        public string PlateNumber { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
        public double MaxTrunkWeight { get; set; }
        public double CurrentTrunkWeight { get; set; }
        public double TrunkVolume { get; set; }
        public double TrunkHeight { get; set; }
        public double TrunkWidth { get; set; }
        public double TrunkLength { get; set; }
        public double CurrentTrunkVolume { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public DateTime CarRegistrationDate { get; set; }
        public VehicleState VehicleState { get; set; }
        public DateTime TechnicalReviewDate { get; set; }
        public VehicleModel VehicleModel { get; set; }
        public int VehicleModelId { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public PickUpPoint PickUpPoint { get; set; }
        public int PickUpPointId { get; set; }
    }
}
