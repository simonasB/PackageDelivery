using System.Collections.Generic;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Services.Maps.Google.Directions.Responses;

namespace PackageDelivery.WebApplication.Services
{
    public interface IRouteCalculator {
        long CalculateRouteTime(
            Address origin, Address destination, IEnumerable<Address> waypoints);
    }
}
