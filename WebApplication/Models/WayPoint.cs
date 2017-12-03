using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PackageDelivery.WebApplication.Models
{
    [DataContract]
    public class WayPoint
    {
        [DataMember(Name = "location")]
        public string Location { get; set; }
        [DataMember(Name = "stopover")]
        public bool StopOver { get; set; }
    }
}
