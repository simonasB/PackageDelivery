using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PackageDelivery.WebApplication.Models
{
    [DataContract]
    public class DirectionsRequest
    {
        [DataMember(Name = "origin")]
        public string Origin { get; set; }
        [DataMember(Name = "destination")]
        public string Destination { get; set; }
        [DataMember(Name = "waypoints")]
        public List<WayPoint> WayPoints { get; set; }
        [DataMember(Name = "travelMode")]
        public string TravelMode { get; set; }
    }
}
