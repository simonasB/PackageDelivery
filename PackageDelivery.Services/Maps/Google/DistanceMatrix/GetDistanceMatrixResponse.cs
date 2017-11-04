using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PackageDelivery.WebApplication.Services.Maps.Google.DistanceMatrix {
    /// <summary>
    /// Get distance matrix response
    /// </summary>
    [DataContract]
    public class GetDistanceMatrixResponse
    {

        #region Properties

        /// <summary>
        /// Origin addresses
        /// </summary>
        [DataMember(Name = "origin_addresses")]
        public List<string> OriginAddresses { get; set; }

        /// <summary>
        /// Destination addresses
        /// </summary>
        [DataMember(Name = "destination_addresses")]
        public List<string> DestinationAddresses { get; set; }

        /// <summary>
        /// Rows
        /// </summary>
        [DataMember(Name = "rows")]
        public List<DistanceMatrixRow> Rows { get; set; }

        #endregion

    }
}
