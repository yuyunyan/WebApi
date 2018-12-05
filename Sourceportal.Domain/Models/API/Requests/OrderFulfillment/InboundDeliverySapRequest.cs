using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.OrderFulfillment
{
    [DataContract]
    public class InboundDeliverySapRequest
    {
        [DataMember(Name = "inboundDeliveryItems")]
        public List<InboundDeliveryItem> InboundDeliveryItems { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }

    [DataContract]
    public class InboundDeliveryItem
    {
        [DataMember(Name = "poLineNum")]
        public string PoLineNum { get; set; }

        [DataMember(Name = "poExternalId")]
        public string PoExternalId { get; set; }

        [DataMember(Name = "itemExternalId")]
        public string ItemExternalId { get; set; }

        [DataMember(Name = "identifiedStockExternalId")]
        public string IdentifiedStockExternalId { get; set; }

        [DataMember(Name = "recvDate")]
        public DateTime RecvDate { get; set; }

        [DataMember(Name = "inventoryList")]
        public List<InventoryDetails> InventoryList { get; set; }

        [DataMember(Name = "stockStatusExternalId")]
        public string StockStatusExternalId { get; set; }

        [DataMember(Name = "productSpec")]
        public string ProductSpec { get; set; }
    }

    public class InventoryDetails
    {
        public string WarehouseBinID { get; set; }
        public int Qty { get; set; }
        public string WarehouseUUID { get; set; }
        public bool IsRestricted { get; set; }
        public bool IsInspection { get; set; }
    }
}
