using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PackageDelivery.WebApplication.Services.Maps.Google.Directions.Responses;

namespace PackageDelivery.WebApplication.Services.Maps {
    public interface IMapsGeneratorService {
        string CreateStaticMapUrl(GetDirectionsResponse getDirectionsResponse);
    }
}
