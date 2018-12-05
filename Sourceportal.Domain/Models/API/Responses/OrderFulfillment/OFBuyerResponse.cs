using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.OrderFulfillment
{
    [DataContract]
    public class OFBuyerResponse
    {
        [DataMember(Name = "soLineId")]
        public int SOLineID { get; set; }
        [DataMember(Name = "buyerName")]
        public string BuyerName { get; set; }
    }

    public class OFBuyerMap
    {
        public int SOLineID { get; set; }
        public string BuyerName { get; set; }
    }
}
