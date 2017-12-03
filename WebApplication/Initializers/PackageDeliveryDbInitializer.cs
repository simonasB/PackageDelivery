using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Core;
using PackageDelivery.Data;
using PackageDelivery.Domain.Entities;
using PackageDelivery.Services.Maps;
using PackageDelivery.WebApplication.Authorization;

namespace PackageDelivery.WebApplication.Initializers {
    public class PackageDeliveryDbInitializer {
        private readonly PackageDeliveryContext _ctx;
        private readonly UserManager<User> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;
        private readonly IDistanceCalculator _distanceCalculator;

        public PackageDeliveryDbInitializer(PackageDeliveryContext ctx, UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr, IDistanceCalculator distanceCalculator) {
            _ctx = ctx;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            _distanceCalculator = distanceCalculator;
        }

        public async Task CustomSeed() {

            var user = await _userMgr.FindByEmailAsync("super@admin.com");

            await _userMgr.RemoveClaimAsync(user, new Claim(Claims.Role, UserRoles.ADMIN));
            /*var userManager = new User {
                UserName = "super@admin.com",
                FirstName = "super",
                LastName = "admin",
                Email = "super@admin.com",
                IsActive = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                LockoutEnabled = false
            };

            var resultUsr = await _userMgr.CreateAsync(userManager, "P@assw0rd!");
            var resultRole = await _userMgr.AddClaimAsync(userManager, new Claim(Claims.Role, UserRoles.SUPER_ADMIN));*/
        }

        public async Task Seed() {
            #region Common

            if (true) {
                _ctx.Database.EnsureDeleted();
                _ctx.Database.Migrate();
                var country = new Country {
                    Name = "Lithuania",
                    RegistrationDate = DateTime.UtcNow
                };


                var currency = new Currency {
                    Name = "Eur"
                };

                var paymentMethod = new PaymentMethod {
                    IsActive = true,
                    Name = "Paysera",
                    RegistrationDate = DateTime.UtcNow
                };

                var vehicleMake = new VehicleMake {
                    Name = "Volkswagen"
                };

                _ctx.Countries.Add(country);
                _ctx.Currencies.Add(currency);
                _ctx.PaymentMethods.Add(paymentMethod);
                _ctx.VehicleMakes.Add(vehicleMake);

                await _ctx.SaveChangesAsync();

                var vehicleModel = new VehicleModel {
                    Name = "Transporter",
                    VehicleMakeId = _ctx.VehicleMakes.Single(o => o.Name == vehicleMake.Name).VehicleMakeId
                };

                _ctx.VehicleModels.Add(vehicleModel);

                #endregion

                #region Companies

                #region cheap

                var companyCheap = new Company {
                    CubePrice = 100,
                    CurrencyId = _ctx.Currencies.Single(o => o.Name == currency.Name).CurrencyId,
                    Email = "email@email.com",
                    IsActive = true,
                    KilogramPrice = 100,
                    Name = "CompanyCheap",
                    RegistrationDate = DateTime.UtcNow,
                    KilometerPrice = 100,
                    VolumeAndWeightRatioToApplyVolumePrice = 5
                };

                _ctx.Companies.Add(companyCheap);
                await _ctx.SaveChangesAsync();

                #region PickUpPoints


                var addressCompanyCPickupPointVilnius = new Address {
                    City = "Vilnius",
                    HouseNumber = "53",
                    ApartmentNumber = 2,
                    PostalCode = "LT-03210",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Algirdo g.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                var addressCompanyCPickupPointKaunas = new Address {
                    City = "Kaunas",
                    HouseNumber = "77",
                    ApartmentNumber = 6,
                    PostalCode = "LT-03211",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Taikos pr.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                var addressCompanyCPickupPointAlytus = new Address {
                    City = "Alytus",
                    HouseNumber = "17",
                    ApartmentNumber = 7,
                    PostalCode = "LT-62375",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Kepyklos g.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                _ctx.Addresses.Add(addressCompanyCPickupPointVilnius);
                _ctx.Addresses.Add(addressCompanyCPickupPointKaunas);
                _ctx.Addresses.Add(addressCompanyCPickupPointAlytus);

                await _ctx.SaveChangesAsync();

                var pickUpPointCVilnius = new PickUpPoint {
                    Name = "CheapVilnius",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyCheap.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyCPickupPointVilnius.StreetName
                             && o.HouseNumber == addressCompanyCPickupPointVilnius.HouseNumber).AddressId
                };

                var pickUpPointCKaunas = new PickUpPoint {
                    Name = "CheapKaunas",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyCheap.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyCPickupPointKaunas.StreetName
                             && o.HouseNumber == addressCompanyCPickupPointKaunas.HouseNumber).AddressId
                };

                var pickUpPointCAlytus = new PickUpPoint {
                    Name = "CheapAlytus",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyCheap.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyCPickupPointAlytus.StreetName
                             && o.HouseNumber == addressCompanyCPickupPointAlytus.HouseNumber).AddressId
                };

                _ctx.PickUpPoints.Add(pickUpPointCVilnius);
                _ctx.PickUpPoints.Add(pickUpPointCKaunas);
                _ctx.PickUpPoints.Add(pickUpPointCAlytus);

                await _ctx.SaveChangesAsync();

                #endregion

                #region vehicles

                var vehicleCPlateNumbers = new List<List<string>> {
                    new List<string> {
                        "CheapSmallKaunas",
                        "CheapSmallVilnius",
                        "CheapSmallAlytus"
                    },
                    new List<string> {
                        "CheapMediumKauans",
                        "CheapMediumVilnius",
                        "CheapMediumAlytus"
                    },
                    new List<string> {
                        "CheapBigKaunas",
                        "CheapBigVilnius",
                        "CheapBigAlytus"
                    },
                };

                // 3 Vehicles for each pickup point

                foreach (var vehiclePlateNumber in vehicleCPlateNumbers) {
                    var vehicleCheapSmall = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[0],
                        Color = "White",
                        Weight = 2000,
                        MaxTrunkWeight = 1000,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 2000000,
                        TrunkLength = 200,
                        TrunkWidth = 100,
                        TrunkHeight = 100,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyCheap.Name).CompanyId
                    };

                    var vehicleCheapMedium = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[1],
                        Color = "White",
                        Weight = 2100,
                        MaxTrunkWeight = 1100,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 3515625,
                        TrunkLength = 225,
                        TrunkWidth = 125,
                        TrunkHeight = 125,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCVilnius.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyCheap.Name).CompanyId
                    };

                    var vehicleCheapBig = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[2],
                        Color = "White",
                        Weight = 2200,
                        MaxTrunkWeight = 1300,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 5625000,
                        TrunkLength = 250,
                        TrunkWidth = 150,
                        TrunkHeight = 150,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCAlytus.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyCheap.Name).CompanyId
                    };

                    _ctx.Vehicles.Add(vehicleCheapSmall);
                    _ctx.Vehicles.Add(vehicleCheapMedium);
                    _ctx.Vehicles.Add(vehicleCheapBig);
                }

                await _ctx.SaveChangesAsync();

                #endregion

                #endregion

                #region average

                var companyAverage = new Company {
                    CubePrice = 200,
                    CurrencyId = _ctx.Currencies.Single(o => o.Name == currency.Name).CurrencyId,
                    Email = "email2@email.com",
                    IsActive = true,
                    KilogramPrice = 200,
                    Name = "CompanyAverage",
                    RegistrationDate = DateTime.UtcNow,
                    KilometerPrice = 200,
                    VolumeAndWeightRatioToApplyVolumePrice = 4
                };

                _ctx.Companies.Add(companyAverage);
                await _ctx.SaveChangesAsync();

                #region PickUpPoints


                var addressCompanyAPickupPointVilnius = new Address {
                    City = "Vilnius",
                    HouseNumber = "52",
                    ApartmentNumber = 2,
                    PostalCode = "LT-03210",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Algirdo g.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                var addressCompanyAPickupPointKaunas = new Address {
                    City = "Kaunas",
                    HouseNumber = "76",
                    ApartmentNumber = 6,
                    PostalCode = "LT-03211",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Taikos pr.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                var addressCompanyAPickupPointAlytus = new Address {
                    City = "Alytus",
                    HouseNumber = "16",
                    ApartmentNumber = 7,
                    PostalCode = "LT-62375",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Kepyklos g.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                _ctx.Addresses.Add(addressCompanyAPickupPointVilnius);
                _ctx.Addresses.Add(addressCompanyAPickupPointKaunas);
                _ctx.Addresses.Add(addressCompanyAPickupPointAlytus);

                await _ctx.SaveChangesAsync();

                var pickUpPointAVilnius = new PickUpPoint {
                    Name = "AVilnius",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyAverage.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyAPickupPointVilnius.StreetName
                             && o.HouseNumber == addressCompanyAPickupPointVilnius.HouseNumber).AddressId
                };

                var pickUpPointAKaunas = new PickUpPoint {
                    Name = "AKaunas",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyAverage.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyAPickupPointKaunas.StreetName
                             && o.HouseNumber == addressCompanyAPickupPointKaunas.HouseNumber).AddressId
                };

                var pickUpPointAAlytus = new PickUpPoint {
                    Name = "AAlytus",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyAverage.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyAPickupPointAlytus.StreetName
                             && o.HouseNumber == addressCompanyAPickupPointAlytus.HouseNumber).AddressId
                };

                _ctx.PickUpPoints.Add(pickUpPointAVilnius);
                _ctx.PickUpPoints.Add(pickUpPointAKaunas);
                _ctx.PickUpPoints.Add(pickUpPointAAlytus);

                await _ctx.SaveChangesAsync();

                #endregion

                #region vehicles

                var vehicleAPlateNumbers = new List<List<string>> {
                    new List<string> {
                        "ASmallKaunas",
                        "ASmallVilnius",
                        "ASmallAlytus"
                    },
                    new List<string> {
                        "AMediumKauans",
                        "AMediumVilnius",
                        "AMediumAlytus"
                    },
                    new List<string> {
                        "ABigKaunas",
                        "ABigVilnius",
                        "ABigAlytus"
                    },
                };

                // 3 Vehicles for each pickup point

                foreach (var vehiclePlateNumber in vehicleAPlateNumbers) {
                    var vehicleCheapSmall = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[0],
                        Color = "White",
                        Weight = 2000,
                        MaxTrunkWeight = 1000,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 2000000,
                        TrunkLength = 200,
                        TrunkWidth = 100,
                        TrunkHeight = 100,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointAKaunas.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyAverage.Name).CompanyId
                    };

                    var vehicleCheapMedium = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[1],
                        Color = "White",
                        Weight = 2100,
                        MaxTrunkWeight = 1100,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 3515625,
                        TrunkLength = 225,
                        TrunkWidth = 125,
                        TrunkHeight = 125,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointAVilnius.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyAverage.Name).CompanyId
                    };

                    var vehicleCheapBig = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[2],
                        Color = "White",
                        Weight = 2200,
                        MaxTrunkWeight = 1300,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 5625000,
                        TrunkLength = 250,
                        TrunkWidth = 150,
                        TrunkHeight = 150,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointAAlytus.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyAverage.Name).CompanyId
                    };

                    _ctx.Vehicles.Add(vehicleCheapSmall);
                    _ctx.Vehicles.Add(vehicleCheapMedium);
                    _ctx.Vehicles.Add(vehicleCheapBig);
                }

                await _ctx.SaveChangesAsync();

                #endregion

                #endregion

                #region expensive

                var companyExpensive = new Company {
                    CubePrice = 300,
                    CurrencyId = _ctx.Currencies.Single(o => o.Name == currency.Name).CurrencyId,
                    Email = "email3@email.com",
                    IsActive = true,
                    KilogramPrice = 300,
                    Name = "CompanyExpensive",
                    RegistrationDate = DateTime.UtcNow,
                    KilometerPrice = 300,
                    VolumeAndWeightRatioToApplyVolumePrice = 2.5
                };

                _ctx.Companies.Add(companyExpensive);
                await _ctx.SaveChangesAsync();

                #region PickUpPoints


                var addressCompanyEPickupPointVilnius = new Address {
                    City = "Vilnius",
                    HouseNumber = "51",
                    ApartmentNumber = 2,
                    PostalCode = "LT-03210",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Algirdo g.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                var addressCompanyEPickupPointKaunas = new Address {
                    City = "Kaunas",
                    HouseNumber = "75",
                    ApartmentNumber = 6,
                    PostalCode = "LT-03211",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Taikos pr.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                var addressCompanyEPickupPointAlytus = new Address {
                    City = "Alytus",
                    HouseNumber = "15",
                    ApartmentNumber = 7,
                    PostalCode = "LT-62375",
                    RegistrationDate = DateTime.UtcNow,
                    StreetName = "Kepyklos g.",
                    CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                };

                _ctx.Addresses.Add(addressCompanyEPickupPointVilnius);
                _ctx.Addresses.Add(addressCompanyEPickupPointKaunas);
                _ctx.Addresses.Add(addressCompanyEPickupPointAlytus);

                await _ctx.SaveChangesAsync();

                var pickUpPointEVilnius = new PickUpPoint {
                    Name = "EVilnius",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyExpensive.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyEPickupPointVilnius.StreetName
                             && o.HouseNumber == addressCompanyEPickupPointVilnius.HouseNumber).AddressId
                };

                var pickUpPointEKaunas = new PickUpPoint {
                    Name = "EKaunas",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyExpensive.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyEPickupPointKaunas.StreetName
                             && o.HouseNumber == addressCompanyEPickupPointKaunas.HouseNumber).AddressId
                };

                var pickUpPointEAlytus = new PickUpPoint {
                    Name = "EAlytus",
                    RegistrationDate = DateTime.UtcNow,
                    IsStatic = true,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyExpensive.Name).CompanyId,
                    AddressId = _ctx.Addresses.Single(
                        o => o.StreetName == addressCompanyEPickupPointAlytus.StreetName
                             && o.HouseNumber == addressCompanyEPickupPointAlytus.HouseNumber).AddressId
                };

                _ctx.PickUpPoints.Add(pickUpPointEVilnius);
                _ctx.PickUpPoints.Add(pickUpPointEKaunas);
                _ctx.PickUpPoints.Add(pickUpPointEAlytus);

                await _ctx.SaveChangesAsync();

                #endregion

                #region vehicles

                var vehicleEPlateNumbers = new List<List<string>> {
                    new List<string> {
                        "ESmallKaunas",
                        "ESmallVilnius",
                        "ESmallAlytus"
                    },
                    new List<string> {
                        "EMediumKauans",
                        "EMediumVilnius",
                        "EMediumAlytus"
                    },
                    new List<string> {
                        "EBigKaunas",
                        "EBigVilnius",
                        "EBigAlytus"
                    },
                };

                // 3 Vehicles for each pickup point

                foreach (var vehiclePlateNumber in vehicleEPlateNumbers) {
                    var vehicleCheapSmall = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[0],
                        Color = "White",
                        Weight = 2000,
                        MaxTrunkWeight = 1000,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 2000000,
                        TrunkLength = 200,
                        TrunkWidth = 100,
                        TrunkHeight = 100,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointEKaunas.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyExpensive.Name).CompanyId
                    };

                    var vehicleCheapMedium = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[1],
                        Color = "White",
                        Weight = 2100,
                        MaxTrunkWeight = 1100,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 3515625,
                        TrunkLength = 225,
                        TrunkWidth = 125,
                        TrunkHeight = 125,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointEVilnius.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyExpensive.Name).CompanyId
                    };

                    var vehicleCheapBig = new Vehicle {
                        IsActive = true,
                        PlateNumber = vehiclePlateNumber[2],
                        Color = "White",
                        Weight = 2200,
                        MaxTrunkWeight = 1300,
                        CurrentTrunkWeight = 0,
                        CurrentTrunkVolume = 0,
                        TrunkVolume = 5625000,
                        TrunkLength = 250,
                        TrunkWidth = 150,
                        TrunkHeight = 150,
                        DateOfManufacture = DateTime.UtcNow,
                        CarRegistrationDate = DateTime.UtcNow,
                        VehicleState = VehicleState.AtParkingLot,
                        TechnicalReviewDate = DateTime.UtcNow,
                        VehicleModelId = _ctx.VehicleModels.Single(o => o.Name == vehicleModel.Name).VehicleModelId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointEAlytus.Name).PickUpPointId,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyExpensive.Name).CompanyId
                    };

                    _ctx.Vehicles.Add(vehicleCheapSmall);
                    _ctx.Vehicles.Add(vehicleCheapMedium);
                    _ctx.Vehicles.Add(vehicleCheapBig);
                }

                await _ctx.SaveChangesAsync();

                #endregion

                #endregion

                #endregion

                #region Orders

                var order = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 100,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 90,
                            Length = 190,
                            Width = 80,
                            Volume = 1368000,
                            Weight = 500
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Raudondvaris",
                        StreetName = "Vejuonos g.",
                        HouseNumber = "16",
                        ApartmentNumber = 5,
                        PostalCode = "LT-54117",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Neveronys",
                        StreetName = "Kertupio g.",
                        HouseNumber = "27",
                        ApartmentNumber = 5,
                        PostalCode = "LT-54487",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order2 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 15,
                            Length = 15,
                            Width = 15,
                            Volume = 3375,
                            Weight = 30
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Domeikava",
                        StreetName = "Saulėtekio g.",
                        HouseNumber = "10",
                        ApartmentNumber = 5,
                        PostalCode = "LT-54350",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Ramučiai",
                        StreetName = "Dainavos g.",
                        HouseNumber = "2",
                        ApartmentNumber = 5,
                        PostalCode = "LT-54466",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order3 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 20,
                            Length = 20,
                            Width = 20,
                            Volume = 8000,
                            Weight = 20
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Karmėlava",
                        StreetName = "Piliakalnio g.",
                        HouseNumber = "7",
                        ApartmentNumber = 5,
                        PostalCode = "LT-54450",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Ringaudai",
                        StreetName = "Rasos g.",
                        HouseNumber = "10",
                        ApartmentNumber = 5,
                        PostalCode = "LT-53331",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order4 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 30,
                            Length = 30,
                            Width = 30,
                            Volume = 9000,
                            Weight = 0.1
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Garliava",
                        StreetName = "Maironio g.",
                        HouseNumber = "26",
                        ApartmentNumber = 5,
                        PostalCode = "LT-53257",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Raseiniai",
                        StreetName = "Trumpoji g.",
                        HouseNumber = "4",
                        ApartmentNumber = 5,
                        PostalCode = "LT-60179",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order5 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 5,
                            Length = 5,
                            Width = 5,
                            Volume = 125,
                            Weight = 1
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Sakiai",
                        StreetName = "Turgaus g.",
                        HouseNumber = "12",
                        ApartmentNumber = 5,
                        PostalCode = "LT-71121",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Kėdainiai",
                        StreetName = "Liaudies g.",
                        HouseNumber = "19",
                        ApartmentNumber = 5,
                        PostalCode = "LT-57414",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order6 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 5,
                            Length = 5,
                            Width = 5,
                            Volume = 125,
                            Weight = 0.5
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Rumšiškės",
                        StreetName = "P. Cvirkos g.",
                        HouseNumber = "1",
                        ApartmentNumber = 5,
                        PostalCode = "LT-56336",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Lapės",
                        StreetName = "Vilties g.",
                        HouseNumber = "28",
                        ApartmentNumber = 5,
                        PostalCode = "LT-54435",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order7 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 8,
                            Length = 8,
                            Width = 10,
                            Volume = 640,
                            Weight = 2
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Garliava",
                        StreetName = "Taikos g.",
                        HouseNumber = "3",
                        ApartmentNumber = 5,
                        PostalCode = "LT-532251",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Kaunas",
                        StreetName = "Trimito g.",
                        HouseNumber = "7",
                        ApartmentNumber = 5,
                        PostalCode = "LT-44293",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order8 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 30,
                            Length = 40,
                            Width = 20,
                            Volume = 24000,
                            Weight = 30
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Garliava",
                        StreetName = "Taikos g.",
                        HouseNumber = "3",
                        ApartmentNumber = 5,
                        PostalCode = "LT-532251",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Kaunas",
                        StreetName = "Trimito g.",
                        HouseNumber = "7",
                        ApartmentNumber = 5,
                        PostalCode = "LT-44293",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order9 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 50,
                            Length = 70,
                            Width = 60,
                            Volume = 210000,
                            Weight = 50
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Kaunas",
                        StreetName = "Partizanų g.",
                        HouseNumber = "28",
                        ApartmentNumber = 5,
                        PostalCode = "LT-49493",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Molėtai",
                        StreetName = "Melioratorių g.",
                        HouseNumber = "19",
                        ApartmentNumber = 5,
                        PostalCode = "LT-33159",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order10 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 10,
                            Length = 10,
                            Width = 10,
                            Volume = 1000,
                            Weight = 0.5
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Kaunas",
                        StreetName = "V. Krėvės pr.",
                        HouseNumber = "77",
                        ApartmentNumber = 5,
                        PostalCode = "LT-50360",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Ukmergė",
                        StreetName = "Pavasario g.",
                        HouseNumber = "7",
                        ApartmentNumber = 5,
                        PostalCode = "LT-20138",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var order11 = new Order {
                    OrderState = OrderState.ReadyToBePickedUp,
                    Payment = new Payment {
                        PaidDate = DateTime.UtcNow,
                        PaymentState = PaymentState.Paid,
                        Price = 50,
                        PaymentMethodId = _ctx.PaymentMethods.Single(o => o.Name == paymentMethod.Name).PaymentMethodId
                    },
                    RegistrationDate = DateTime.UtcNow,
                    Items = new List<Item> {
                        new Item {
                            Description = "desc",
                            Height = 80,
                            Length = 190,
                            Width = 80,
                            Volume = 1216000,
                            Weight = 300
                        }
                    },
                    PickUpAddress = new Address {
                        City = "Kaunas",
                        StreetName = "Bartuvos g.",
                        HouseNumber = "14",
                        ApartmentNumber = 5,
                        PostalCode = "LT-47133",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    DeliveryAddress = new Address {
                        City = "Vilkaviškis",
                        StreetName = "Vilniaus g.",
                        HouseNumber = "20",
                        ApartmentNumber = 5,
                        PostalCode = "LT-70142",
                        RegistrationDate = DateTime.UtcNow,
                        CountryId = _ctx.Countries.Single(o => o.Name == country.Name).CountryId
                    },
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                };

                var ordersList = new List<Order> {
                    order,
                    order2,
                    order3,
                    order4,
                    order5,
                    order6,
                    order7,
                    order8,
                    order9,
                    order10,
                    order11
                };

                foreach (var orderInList in ordersList) {
                    orderInList.SetOrderMeasurements();
                    orderInList.DistanceBetweenPickUpAndDeliveryAddresses =
                        _distanceCalculator.CalculateDistanceBetweenTwoPoints(orderInList.PickUpAddress,
                            orderInList.DeliveryAddress);
                    orderInList.DistanceBetweenPickUpPointAndDeliveryAddress =
                        _distanceCalculator.CalculateDistanceBetweenTwoPoints(orderInList.DeliveryAddress,
                            pickUpPointCKaunas.Address);
                }

                _ctx.AddRange(ordersList);

                await _ctx.SaveChangesAsync();

                var savedOrder = _ctx.Orders.Single(o => o.PickUpAddress.City == "Domeikava");

                #endregion

                #region Users

                var userCustomer = new User {
                    UserName = "a@b.com",
                    FirstName = "a",
                    LastName = "b",
                    Email = "a@b.com",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                };

                var userResult = await _userMgr.CreateAsync(userCustomer, "P@assw0rd!");
                var roleResult = await _userMgr.AddClaimAsync(userCustomer, new Claim(Claims.Role, UserRoles.CUSTOMER));
                //var claimResult = await _userMgr.AddClaimAsync(userCustomer, new Claim("SuperUser", "True"));

                var driverCKaunasUserNames = new[]
                    {"driverCKaunas@a.com", "driver2CKaunas@a.com", "driver3CKaunas@a.com"};

                foreach (var driverUserName in driverCKaunasUserNames) {
                    var userDriver = new User {
                        UserName = driverUserName,
                        FirstName = "a",
                        LastName = "b",
                        Email = driverUserName,
                        IsActive = true,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        LockoutEnabled = false,
                        CompanyId = _ctx.Companies.Single(o => o.Name == companyCheap.Name).CompanyId,
                        PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId
                    };

                    await _userMgr.CreateAsync(userDriver, "P@assw0rd!");
                    await _userMgr.AddClaimAsync(userDriver, new Claim(Claims.Role, UserRoles.DRIVER));
                }

                var userManager = new User {
                    UserName = "managerCKaunas@b.com",
                    FirstName = "aa",
                    LastName = "bb",
                    Email = "managerCKaunas@b.com",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    CompanyId = _ctx.Companies.Single(o => o.Name == companyCheap.Name).CompanyId,
                    PickUpPointId = _ctx.PickUpPoints.Single(o => o.Name == pickUpPointCKaunas.Name).PickUpPointId,
                };

                var resultUsr = await _userMgr.CreateAsync(userManager, "P@assw0rd!");
                var resultRole = await _userMgr.AddClaimAsync(userManager, new Claim(Claims.Role, UserRoles.MANAGER));

                var userManagerWithClaims = new User
                {
                    UserName = "managerCKaunas@withclaims.com",
                    FirstName = "aawithclaims",
                    LastName = "bbwithclaims",
                    Email = "managerCKaunas@withclaims.com",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    CompanyId = 1,
                    PickUpPointId = 1
                };

                userManagerWithClaims.Claims.Add(new IdentityUserClaim<string>
                {
                    ClaimType = nameof(User.CompanyId),
                    ClaimValue = userManager.CompanyId.ToString()
                });

                userManagerWithClaims.Claims.Add(new IdentityUserClaim<string>
                {
                    ClaimType = nameof(User.PickUpPointId),
                    ClaimValue = userManager.PickUpPointId.ToString()
                });

                userManagerWithClaims.Claims.Add(new IdentityUserClaim<string>
                {
                    ClaimType = "email",
                    ClaimValue = userManager.Email
                });

                await _userMgr.CreateAsync(userManagerWithClaims, "P@assw0rd!");
                await _userMgr.AddClaimAsync(userManagerWithClaims, new Claim(Claims.Role, UserRoles.MANAGER));

                #endregion
            }
        }
    }
}
