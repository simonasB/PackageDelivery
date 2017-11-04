using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PackageDelivery.Domain.Entities;
using PackageDelivery.SharedKernel.Data.Maps;
using PackageDelivery.WebApplication.Services.Maps.Google.Directions.Responses;
using PackageDelivery.WebApplication.Utils;

namespace PackageDelivery.WebApplication.Services
{
    public class GoogleRouteCalculator : GoogleServicesBase
    {
        public GoogleRouteCalculator(HttpClient httpClient) : base(httpClient) {
        }

        public long GetRouteTime(string origin, string destination, List<string> waypoints) {
            var directions = GetDirections(origin, destination, waypoints);

            long routeTime = 0;

            if (directions.IsValid)
            {
                routeTime = directions.Routes.First().Legs.Sum(o => o.Duration.Value);
            }

            return routeTime;
        }

        private GetDirectionsResponse GetDirections(string origin, string destination, IEnumerable<string> waypoints)
        {
            var wayPointsAsStrings = Converter.JoinList(waypoints);
            var uri = new Uri($"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&waypoints={wayPointsAsStrings}&key={APIKey}");

            var result = ExecuteRequest<GetDirectionsResponse>(uri);

            return result;
        }
    }
}
