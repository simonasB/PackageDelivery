﻿using System.Runtime.Serialization;

namespace PackageDelivery.Services.Maps.Google.Directions.Enums
{

    /// <summary>
    /// Transit routing preference enum
    /// </summary>
    public enum TransitRoutingPreferenceEnum
    {
        /// <summary>
        /// Prefer limited amounts of walking
        /// </summary>
        [EnumMember(Value = "less_walking")]
        LessWalking,
        /// <summary>
        /// Prefer a limited number of transfers
        /// </summary>
        [EnumMember(Value = "fewer_transfers")]
        FewerTransfers
    }
}