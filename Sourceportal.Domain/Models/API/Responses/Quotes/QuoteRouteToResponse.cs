using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
    public class QuoteRouteToResponse
    {
        [DataMember(Name = "quoteLineId")]
        public int QuoteLineID { get; set; }
        [DataMember(Name = "routeStatusId")]
        public int RouteStatusID { get; set; }
        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }
        [DataMember(Name = "icon")]
        public string Icon { get; set; }
        [DataMember(Name = "iconColor")]
        public string IconColor { get; set; }
        [DataMember(Name = "buyerName")]
        public string BuyerName { get; set; }
        [DataMember(Name = "buyerInitials")]
        public string BuyerInitials { get; set; }
    }

    public class QuoteRouteToMap
    {
        public int QuoteLineID { get; set; }
        public int RouteStatusID { get; set; }
        public string StatusName { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string BuyerName { get; set; }
    }
}
