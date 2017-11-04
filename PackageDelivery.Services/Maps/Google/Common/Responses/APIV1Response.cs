using System.Runtime.Serialization;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Common.Responses
{
    /// <summary>
    /// API Version 1 response
    /// </summary>
    [DataContract]
    public abstract class APIV1Response : APIResponse
    {

        #region Constructors

        /// <summary>
        /// Create a new instance
        /// </summary>
        protected APIV1Response()
        {

            // Always set status as OK since it's Version 1 of the API
            Status = "OK";

        }

        #endregion

    }
}