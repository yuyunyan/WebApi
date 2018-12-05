using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Quotes
{
    [DataContract]
    public class RouteQuoteLineRequest
    {
        [DataMember(Name = "buyerIds")]
        public List<RouteBuyerIds> BuyerIDs { get; set; }
        [DataMember(Name = "isSpecificBuyer")]
        public bool IsSpecificBuyer { get; set; }
        [DataMember(Name = "quoteLineIds")]
        public List<RouteQuoteLine> QuoteLineIds { get; set; }
    }

    public class RouteQuoteLine
    {
        public int QuoteLineID { get; set; }
    }

    public class RouteBuyerIds
    {
        [DataMember(Name = "userId")]
        public int UserID { get; set; }
    }
}
