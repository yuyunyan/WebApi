using Sourceportal.Domain.Models.API.Responses.BOMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemInventoryResponse
    {
        [DataMember(Name = "items")]
        public List<ItemInventoryDetails> Inventory { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }

    [DataContract]
    public class ItemInventoryDetails
    {
        [DataMember(Name = "warehouseId")]
        public int WarehouseID { get; set; }

        [DataMember(Name = "warehouseName")]
        public string WarehouseName { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "origQty")]
        public int OrigQty { get; set; }

        [DataMember(Name = "availableQty")]
        public int AvailableQty { get; set; }

        [DataMember(Name = "purchaseOrderId")]
        public int PurchaseOrderID { get; set; }

        [DataMember(Name = "poVersionId")]
        public int POVersionID { get; set; }

        [DataMember(Name = "poExternalId")]
        public string POExternalID { get; set; }

        [DataMember(Name = "buyer")]
        public string Buyer { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusID { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "cost")]
        public string Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "packageCondition")]
        public string PackageCondition{ get; set; }

        [DataMember(Name = "shipDate")]
        public string ShipDate { get; set; }

        [DataMember(Name = "allocated")]
        public List<AllocationsResponse> Allocated { get; set; }
    }
}

