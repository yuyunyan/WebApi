using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Middleware.OrderFulfillment
{
    [DataContract]
    public class InventoryAllocateSync : MiddlewareSyncBase
    {
        public InventoryAllocateSync(int id, string externalId) : base(id, externalId)
        {
        }

        [DataMember(Name = "identifiedStockExternalId")]
        public string IdentifiedStockExternalId { get; set; }

        [DataMember(Name = "productSpecId")]
        public string ProductSpecId { get; set; }

        [DataMember(Name = "splitLineDetails")]
        public List<SplitLineDetails> SplitLines { get; set; }
    }

    public class SplitLineDetails
    {
        [DataMember(Name = "warehouseExternalId")]
        public string WarehouseExternalId { get; set; }

        [DataMember(Name = "binExternalId")]
        public string BinExternalId { get; set; }

        [DataMember(Name = "binExternalUUID")]
        public string BinExternalUUID { get; set; }

        [DataMember(Name = "qty")]
        public decimal Qty { get; set; }
    }
}
