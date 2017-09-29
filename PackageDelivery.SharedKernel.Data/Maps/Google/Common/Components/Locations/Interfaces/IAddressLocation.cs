using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Common.Interfaces;
using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Interfaces.Combined;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Interfaces
{

    /// <summary>
    /// Address location interface
    /// </summary>
    public interface IAddressLocation : ILocation, IAddressOrGeoCoordinatesLocation
    {

        #region Properties

        /// <summary>
        /// Address value
        /// </summary>
        string Address { get; set; }

        #endregion

    }
}