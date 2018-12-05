using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    [DataContract]
    public class SetPurchaseOrderLineResponse
    {
        [DataMember(Name = "poLineId")]
        public int POLineId { get; set; }

        [DataMember(Name = "lineNo")]
        public int LineNo { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }
    }
}
