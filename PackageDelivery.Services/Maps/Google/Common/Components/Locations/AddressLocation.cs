using System.Runtime.Serialization;
using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Common;
using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations.Interfaces;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Common.Components.Locations
{
    /// <summary>
    /// Address location
    /// </summary>
    [DataContract]
    public class AddressLocation : Location, IAddressLocation
    {

        #region Properties

        /// <summary>
        /// Address value
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance
        /// </summary>
        public AddressLocation()
        {
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="address">Address value</param>
        public AddressLocation(string address)
        {
            Address = address;
        }

        #endregion
    }

}