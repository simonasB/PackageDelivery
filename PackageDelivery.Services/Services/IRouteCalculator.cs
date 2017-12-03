using System.Collections.Generic;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.Services.Services
{
    public interface IRouteCalculator {
        long CalculateRouteTime(
            Address origin, Address destination, IEnumerable<Address> waypoints);
    }
}
