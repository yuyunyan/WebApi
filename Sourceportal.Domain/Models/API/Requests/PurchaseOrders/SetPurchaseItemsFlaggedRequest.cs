using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.PurchaseOrders
{
    [DataContract]
    public class SetPurchaseItemsFlaggedRequest
    {
        [DataMember(Name = "purchaseOrderDetails")]
        public SetPurchaseOrderDetailsRequest PurchaseOrderDetails { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "purchaseOrderLines")]
        public List<SetPurchaseOrderLineRequest> PurchaseOrderLines { get; set; }
    }
}
