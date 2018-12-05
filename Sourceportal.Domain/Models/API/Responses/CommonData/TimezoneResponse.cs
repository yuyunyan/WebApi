using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses
{
    [DataContract]
    public class TimezoneResponse
    {
        [DataMember(Name = "timezones")]
        public IList<TimeZone> TimeZone { get; set; }
    }

    [DataContract]
    public class TimeZone
    {
        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "current_utc_offset")]
        public string current_utc_offset { get; set; }

        [DataMember(Name = "is_currently_dst")]
        public string is_currently_dst { get; set; }
    }

}



