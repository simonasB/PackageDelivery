using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PackageDelivery.WebApplication.Services.Maps.Google.DistanceMatrix
{
    /// <summary>
    /// Distance matrix row component
    /// </summary>
    [DataContract]
    public class DistanceMatrixRow
    {

        #region Properties

        /// <summary>
        /// Elements
        /// </summary>
        [DataMember(Name = "elements")]
        public List<DistanceMatrixRowElement> Elements { get; set; }

        #endregion

    }
}
