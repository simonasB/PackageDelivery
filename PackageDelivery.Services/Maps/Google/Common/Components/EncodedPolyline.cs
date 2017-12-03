using System.Collections.Generic;
using System.Runtime.Serialization;
using PackageDelivery.Services.Maps.Google.Common.Components.Locations;
using PackageDelivery.Services.Maps.Google.Utils;

namespace PackageDelivery.Services.Maps.Google.Common.Components
{

    /// <summary>
    /// Encoded Polyline component
    /// </summary>
    [DataContract]
    public class EncodedPolyline
    {

        #region Properties

        /// <summary>
        /// Encoded polyline representation of the route
        /// </summary>
        [DataMember(Name = "points")]
        public string EncodedPoints { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Decode the encoded polyline points
        /// </summary>
        /// <returns>List of coordinates</returns>
        public List<GeoCoordinatesLocation> DecodePoints()
        {

            return Converter.DecodePolyline(EncodedPoints);

        }

        #endregion

    }
}