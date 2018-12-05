using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    [DataContract]
    public class SourcingRouteStatusesResponse
    {
        [DataMember(Name = "routeStatuses")]
        public List<RouteStatusesResponse> RouteStatuses { get; set; }
    }

    [DataContract]
    public class RouteStatusesResponse
    {
        [DataMember(Name = "routeStatusId")]
        public int RouteStatusID { get; set; }
        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }
        [DataMember(Name = "isDefault")]
        public bool IsDefault { get; set; }
        [DataMember(Name = "isComplete")]
        public bool IsComplete { get; set; }
        [DataMember(Name = "countQuoteLines")]
        public int? CountQuoteLines { get; set; }
    }
}
