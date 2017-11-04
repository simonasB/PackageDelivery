using System.Runtime.Serialization;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Common.Responses
{
    /// <summary>
    /// API single result response
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    [DataContract]
    public abstract class APISingleResultResponse<TResult> : APIResponse
    {

        #region Properties

        /// <summary>
        /// Result
        /// </summary>
        [DataMember(Name = "result")]
        public TResult Result { get; set; }

        #endregion

    }
}