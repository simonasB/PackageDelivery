using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Services.Maps;
using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Common.Interfaces;

namespace PackageDelivery.ShipmentManagement
{
    class Program
    {
        static void Main(string[] args) {
            var country = new Country {
                CountryId = 1,
                Name = "Lithuania",
                RegistrationDate = DateTime.UtcNow
            };

            var item = new Item {
                Description = "desc",
                Height = 100,
                Length = 100,
                Width = 100,
                ItemId = 1,
                OrderId = 1
            };

            var payment = new Payment {
                OrderId = 1,
                PaidDate = DateTime.UtcNow,
                PaymentId = 1,
                PaymentState = PaymentState.Paid,
                Price = 100
            };

            var pickUpAddress = new Address {
                AddressId = 1,
                City = "Kaunas",
                HouseNumber = "77",
                PostalCode = "LT-12345",
                RegistrationDate = DateTime.UtcNow,
                StreetName = "Taikos pr."
            };

            var deliveryAddress = new Address
            {
                AddressId = 1,
                City = "Vilnius",
                HouseNumber = "21A",
                PostalCode = "LT-12345",
                RegistrationDate = DateTime.UtcNow,
                StreetName = "Konstitucijos pr."
            };

            var order = new Order {
                OrderId = 1,
                DeliveryAddressId = deliveryAddress.AddressId,
                PickUpAddressId = pickUpAddress.AddressId,
                //OrderState = OrderState.WaitingForDelivery,
                Payment = payment,
                RegistrationDate = DateTime.UtcNow,
                
            };

            #region Companies

            var currency = new Currency {
                CurrencyId = 1,
                Name = "Eur"
            };

            var companyCheap = new Company {
                CompanyId = 1,
                CubePrice = 100,
                CurrencyId = currency.CurrencyId,
                Email = "email@email.com",
                IsActive = true,
                KilogramPrice = 100,
                Name = "CompanyCheap",
                RegistrationDate = DateTime.UtcNow,
                KilometerPrice = 100,
                VolumeAndWeightRatioToApplyVolumePrice = 5
            };

            var addressCompanyCPickupPointVilnius = new Address {
                City = "Vilnius",
                HouseNumber = "53",
                ApartmentNumber = 2,
                PostalCode = "LT-03210",
                RegistrationDate = DateTime.UtcNow,
                StreetName = "Algirdo g.",
                CountryId = country.CountryId
            };

            var addressCompanyCPickupPointKaunas = new Address {
                AddressId = 2,
                City = "Kaunas",
                HouseNumber = "77",
                ApartmentNumber = 6,
                PostalCode = "LT-03211",
                RegistrationDate = DateTime.UtcNow,
                StreetName = "Taikos pr.",
                CountryId = country.CountryId
            };

            var addressCompanyCPickupPointAlytus = new Address {
                AddressId = 3,
                City = "Alytus",
                HouseNumber = "17",
                ApartmentNumber = 7,
                PostalCode = "LT-62375",
                RegistrationDate = DateTime.UtcNow,
                StreetName = "Kepyklos g.",
                CountryId = country.CountryId
            };

            //var pickUpPoint

            // CompanyAverage
            var companyAverage = new Company
            {
                CompanyId = 2,
                CubePrice = 200,
                CurrencyId = currency.CurrencyId,
                Email = "email2@email.com",
                IsActive = true,
                KilogramPrice = 200,
                Name = "CompanyAverage",
                RegistrationDate = DateTime.UtcNow,
                KilometerPrice = 200,
                VolumeAndWeightRatioToApplyVolumePrice = 4
            };

            var companyExpensive = new Company
            {
                CompanyId = 3,
                CubePrice = 300,
                CurrencyId = currency.CurrencyId,
                Email = "email3@email.com",
                IsActive = true,
                KilogramPrice = 300,
                Name = "CompanyExpensive",
                RegistrationDate = DateTime.UtcNow,
                KilometerPrice = 300,
                VolumeAndWeightRatioToApplyVolumePrice = 2.5
            };

            #endregion
        }

        private void GoogleMapsTest() {
            var mapsService = new GoogleDistanceCalculator(new HttpClient());
            var origin = new Address
            {
                HouseNumber = "77",
                StreetName = "Taikos pr.",
                City = "Kaunas"
            };

            var destination = new Address
            {
                HouseNumber = "7",
                StreetName = "Mosėdžio g.",
                City = "Kaunas"
            };

            var pickUpPoints = new List<PickUpPoint>();
            var pickUpPoint = new PickUpPoint
            {
                Address = destination
            };

            var pickUpPoint2 = new PickUpPoint
            {
                Address = new Address
                {
                    HouseNumber = "1",
                    StreetName = "Varpo g.",
                    City = "Kaunas"
                }
            };

            var pickUpPoint3 = new PickUpPoint
            {
                Address = new Address
                {
                    HouseNumber = "68",
                    StreetName = "Baršausko g.",
                    City = "Kaunas"
                }
            };

            pickUpPoints.Add(pickUpPoint);
            pickUpPoints.Add(pickUpPoint2);
            pickUpPoints.Add(pickUpPoint3);

            //mapsService.CalculateDistanceBetweenTwoPoints(origin, destination);
            //mapsService.FindNearestPickUpPoint(origin, pickUpPoints);
            //var directions = mapsService.GetDirections(origin, destination, new List<Address> { pickUpPoint2.Address, pickUpPoint3.Address });
        }
    }
}