using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.Data {
    public class PackageDeliveryDbInitializer {
        private readonly PackageDeliveryContext _ctx;

        public PackageDeliveryDbInitializer(PackageDeliveryContext ctx) {
            _ctx = ctx;
        }

        public async Task Seed() {
            if (!_ctx.Vehicles.Any()) {
                _ctx.AddRange(_vehicleMakes);
                await _ctx.SaveChangesAsync();
            }
        }

        private readonly List<VehicleMake> _vehicleMakes = new List<VehicleMake> {
            new VehicleMake {
                IsActive = true,
                Name = "BMW",
                VehicleModels = new List<VehicleModel> {
                    new VehicleModel {
                        Name = "i30",
                        Vehicles = new List<Vehicle> {
                            new Vehicle {
                                DateOfManufacture = DateTime.Now,
                                IsActive = true,
                                PlateNumber = "NBM001",
                                TrunkLength = 100,
                                TrunkVolume = 100,
                                TrunkWidth = 100,
                                VehicleState = VehicleState.AtParkingLot,
                                Weight = 2000,
                            }
                        }
                    }
                }
            }
        };
    }
}
