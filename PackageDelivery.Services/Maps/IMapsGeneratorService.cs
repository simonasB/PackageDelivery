using PackageDelivery.Services.Maps.Google.Directions.Responses;

namespace PackageDelivery.Services.Maps {
    public interface IMapsGeneratorService {
        string CreateStaticMapUrl(GetDirectionsResponse getDirectionsResponse);
    }
}
