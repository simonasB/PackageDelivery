using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Services;
using PackageDelivery.WebApplication.Services.Maps;
using PackageDelivery.WebApplication.Services.Maps.Google.Directions.Responses;

namespace PackageDelivery.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            #region mapstest

            //var mapsService = new GoogleRouteCalculatorAdapter(new HttpClient());
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

            #endregion

            //mapsService.CalculateDistanceBetweenTwoPoints(origin, destination);
            //mapsService.FindNearestPickUpPoint(origin, pickUpPoints);
            //var directions = mapsService.GetDirections(origin, destination, new List<Address> { pickUpPoint2.Address, pickUpPoint3.Address });
            //var deserializedDirections = JsonConvert.DeserializeObject<GetDirectionsResponse>(directions);

            ViewData["Message"] = "Your application description page.";
            //ViewData["Directions"] = directions;

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
