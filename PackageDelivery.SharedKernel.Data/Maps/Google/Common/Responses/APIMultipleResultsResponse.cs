using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Common.Responses
{
    /// <summary>
    /// API multiple results response
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    [DataContract]
    public abstract class APIMultipleResultsResponse<TResult> : APIResponse
    {

        #region Properties

        /// <summary>
        /// Results
        /// </summary>
        [DataMember(Name = "results")]
        public List<TResult> Results { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance
        /// </summary>
        protected APIMultipleResultsResponse()
        {
            Results = new List<TResult>();
        }

        #endregion

    }
}