using System.Runtime.Serialization;

namespace PackageDelivery.WebApplication.Services.Maps.Google.Directions.Enums
{

    /// <summary>
    /// Unit system enum
    /// </summary>
    public enum UnitSystemEnum
    {
        /// <summary>
        /// Metric system. Textual distances are returned using kilometers and meters.
        /// </summary>
        [EnumMember(Value = "metric")]
        Metric,
        /// <summary>
        /// Imperial (English) system. Textual distances are returned using miles and feet.
        /// </summary>
        [EnumMember(Value = "imperial")]
        Imperial
    }
}