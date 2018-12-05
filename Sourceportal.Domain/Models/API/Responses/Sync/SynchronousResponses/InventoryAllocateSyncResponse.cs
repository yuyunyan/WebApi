using Sourceportal.Domain.Models.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sync.SynchronousResponses
{
    [DataContract]
    public class InventoryAllocateSyncResponse : BaseResponse
    {
        [DataMember(Name = "allocatedInventoryList")]
        public List<AllocatedInventoryDetails> AllocatedInventoryList { get; set; }
    }

    public class AllocatedInventoryDetails
    {
        [DataMember(Name = "remainingQty")]
        public decimal RemainingQty { get; set; }

        [DataMember(Name = "oldStockExternalId")]
        public string OldStockExternalId { get; set; }

        [DataMember(Name = "newStockExternalId")]
        public string NewStockExternalId { get; set; }

        [DataMember(Name = "newQty")]
        public decimal NewQty { get; set; }

        [DataMember(Name = "binExternalId")]
        public string BinExternalId { get; set; }

        [DataMember(Name = "warehouseExternalId")]
        public string WarehouseExternalId { get; set; }
    }
}
