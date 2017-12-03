using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.Services.Services
{
    public class GoogleRouteCalculatorAdapter : IRouteCalculator
    {
        private GoogleRouteCalculator _googleRouteCalculator = new GoogleRouteCalculator(new HttpClient());

        public long CalculateRouteTime(Address origin, Address destination, IEnumerable<Address> waypoints) {
            return _googleRouteCalculator.GetRouteTime(origin.ToString(), destination.ToString(),
                waypoints.Select(o => o.ToString()).ToList());
        }
    }
}
