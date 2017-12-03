using System.Globalization;
using System.Runtime.Serialization;
using PackageDelivery.Services.Maps.Google.Common.Components.Locations.Interfaces;
using PackageDelivery.Services.Maps.MapGenerator.Common;

namespace PackageDelivery.Services.Maps.Google.Common.Components.Locations
{

    /// <summary>
    /// Geographic coordinates location
    /// </summary>
    [DataContract]
    public class GeoCoordinatesLocation : Common.Location, IGeoCoordinatesLocation, ILocationString
    {

        #region Properties

        /// <summary>
        /// Latitude value
        /// </summary>
        [DataMember(Name = "lat")]
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude value
        /// </summary>
        [DataMember(Name = "lng")]
        public double Longitude { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance
        /// </summary>
        public GeoCoordinatesLocation()
        {
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        public GeoCoordinatesLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        #endregion

        public string LocationString
        {
            get
            {
                return ToNonScientificString(Latitude) + "," + ToNonScientificString(Longitude);
            }
        }

        public override string ToString()
        {
            return LocationString;
        }

        private static string ToNonScientificString(double d)
        {
            var s = d.ToString(DoubleFormat, CultureInfo.InvariantCulture).TrimEnd('0');
            return s.Length == 0 ? "0.0" : s;
        }

        private static readonly string DoubleFormat = "0." + new string('#', 339);
    }

}