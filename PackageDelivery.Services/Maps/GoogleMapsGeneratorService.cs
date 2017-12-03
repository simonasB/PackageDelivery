using System.Collections.Generic;
using System.Linq;
using PackageDelivery.Services.Maps.Google.Directions.Responses;
using PackageDelivery.Services.Maps.MapGenerator.Common;
using PackageDelivery.Services.Maps.MapGenerator.StaticMaps;
using PackageDelivery.Services.Maps.MapGenerator.StaticMaps.Entities;

namespace PackageDelivery.Services.Maps {
    public class GoogleMapsGeneratorService : IMapsGeneratorService {
        private readonly StaticMapsEngine _staticMapsEngine;

        public GoogleMapsGeneratorService(StaticMapsEngine staticMapsEngine) {
            _staticMapsEngine = staticMapsEngine;
        }
        public string CreateStaticMapUrl(GetDirectionsResponse getDirectionsResponse) {
            //Path from previous directions request
            var steps = getDirectionsResponse.Routes.First().Legs.First().Steps;
            // All start locations
            IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
            // also the end location of the last step
            path.Add(steps.Last().EndLocation);

            string url = _staticMapsEngine.GenerateStaticMapURL(new StaticMapRequest(new Location(38.625223, -90.1646098), 4, new ImageSize(800, 400))
            {
                Pathes = new List<Path>(){ new Path()
                {
                    Style = new PathStyle()
                    {
                        Color = "red"
                    },
                    Locations = path
                }}
            });

            return url;
        }
    }
}
