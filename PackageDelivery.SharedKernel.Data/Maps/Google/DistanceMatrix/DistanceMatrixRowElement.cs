using System.Runtime.Serialization;
using PackageDelivery.WebApplication.Services.Maps.Google.Common.Components;
using PackageDelivery.WebApplication.Services.Maps.Google.Common.Responses;

namespace PackageDelivery.WebApplication.Services.Maps.Google.DistanceMatrix
{
    /// <summary>
    /// Row element component
    /// </summary>
    [DataContract]
    public class DistanceMatrixRowElement : APIResponse {

        #region Properties

        /// <summary>
        /// Duration
        /// </summary>
        [DataMember(Name = "duration")]
        public Duration Duration { get; set; }

        /// <summary>
        /// Distance
        /// </summary>
        [DataMember(Name = "distance")]
        public Duration Distance { get; set; }

        #endregion

    }
}
