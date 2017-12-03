using System.Runtime.Serialization;
using PackageDelivery.Services.Maps.Google.Common.Components.Locations;

namespace PackageDelivery.Services.Maps.Google.Directions.Components
{
    /// <summary>
    /// Transit/stop station component
    /// </summary>
    [DataContract]
    public class TransitStopStation
    {

        #region Properties

        /// <summary>
        /// Name of the transit station/stop
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Location of the transit station/stop
        /// </summary>
        [DataMember(Name = "location")]
        public GeoCoordinatesLocation Location { get; set; }

        #endregion

    }
}