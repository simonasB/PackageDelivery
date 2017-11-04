using System.Collections.Generic;
using System.Runtime.Serialization;
using PackageDelivery.WebApplication.Services.Maps.Google.Directions.Enums;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Directions.Components
{

    /// <summary>
    /// Geocoded waypoint component
    /// </summary>
    [DataContract]
    public class GeocodedWaypoint
    {

        #region Properties

        /// <summary>
        /// Geocoder status
        /// </summary>
        [DataMember(Name = "geocoder_status")]
        public GeocoderStatusEnum GeocoderStatus { get; set; }

        /// <summary>
        /// Place Id
        /// </summary>
        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }

        /// <summary>
        /// Partial match
        /// </summary>
        [DataMember(Name = "partial_match")]
        public string PartialMatch { get; set; }

        /// <summary>
        /// Types
        /// </summary>
        [DataMember(Name = "types")]
        public List<string> Types { get; set; } 

        #endregion
         
    }
}