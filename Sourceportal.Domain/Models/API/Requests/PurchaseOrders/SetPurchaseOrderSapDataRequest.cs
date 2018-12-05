using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.PurchaseOrders
{
    [DataContract]
    public class SetPurchaseOrderSapDataRequest : SetExternalIdRequest
    {
        [DataMember(Name = "items")]
        public List<PurchaseOrderItemsSapData> Items { get; set; }
    }

    [DataContract]
    public class PurchaseOrderItemsSapData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }
}
