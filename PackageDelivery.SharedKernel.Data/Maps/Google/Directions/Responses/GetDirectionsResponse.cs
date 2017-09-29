using System.Collections.Generic;
using System.Runtime.Serialization;
using PackageDelivery.WebApplication.Services.Maps.Google.Common.Responses;
using PackageDelivery.WebApplication.Services.Maps.Google.Directions.Components;
using PackageDelivery.WebApplication.Services.Maps.Google.Directions.Results;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Directions.Responses
{

    /// <summary>
    /// Get directions response
    /// </summary>
    [DataContract]
    public class GetDirectionsResponse : APIResponse
    {

        #region Properties

        /// <summary>
        /// Routes
        /// </summary>
        [DataMember(Name = "routes")]
        public List<GetDirectionsRouteResult> Routes { get; set; }

        /// <summary>
        /// Geocoded waypoints
        /// </summary>
        [DataMember(Name = "geocoded_waypoints")]        
        public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }

        #endregion

    }

}