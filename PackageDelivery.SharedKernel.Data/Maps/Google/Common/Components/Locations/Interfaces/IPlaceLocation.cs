using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Common.Interfaces;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Interfaces
{

    /// <summary>
    /// Place location interface
    /// </summary>
    public interface IPlaceLocation : ILocation
    {

        #region Properties

        /// <summary>
        /// Place Id
        /// </summary>
        string PlaceId { get; set; }

        #endregion

    }
}