using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Sourcing
{
    [DataContract]
    public class SetBuyerRouteRequest
    {
        [DataMember(Name = "routeStatusId")]
        public int RouteStatusID { get; set; }
        [DataMember(Name = "quoteLines")]
        public List<BuyerQuoteLine> QuoteLines { get; set; }
    }

    [DataContract]
    public class BuyerQuoteLine
    {
        [DataMember(Name = "quoteLineId")]
        public int QuoteLineID { get; set; }
    }
}
