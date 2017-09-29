using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Common.Interfaces;
using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Interfaces.Combined;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Interfaces
{

    /// <summary>
    /// Geographic coordinates location interface
    /// </summary>
    public interface IGeoCoordinatesLocation : ILocation, IAddressOrGeoCoordinatesLocation
    {

        #region Properties

        /// <summary>
        /// Latitude
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        double Longitude { get; set; }

        #endregion

    }
}