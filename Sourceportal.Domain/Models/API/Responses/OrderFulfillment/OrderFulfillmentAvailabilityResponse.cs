using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sourceportal.Domain.Models.API.Responses.BOMs;

namespace Sourceportal.Domain.Models.API.Responses.OrderFulfillment
{
    [DataContract]
    public class OrderFulfillmentAvailabilityListResponse
    {
        [DataMember(Name = "availableLines")]
        public IList<OrderFulfillmentAvailabilityResponse> OrderFulfillmentAvailabilityList { get; set; }

        [DataMember(Name = "totalRows")]
        public int TotalRows;
    }

    [DataContract]
    public class OrderFulfillmentAvailabilityResponse
    {
        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }

        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "supplier")]
        public string Supplier { get; set; }

        [DataMember(Name = "supplierId")]
        public int SupplierId { get; set; }

        [DataMember(Name = "originalQty")]
        public int OriginalQty { get; set; }

        [DataMember(Name = "availableQty")]
        public int AvailableQty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "shipDate")]
        public DateTime ShipDate { get; set; }

        [DataMember(Name = "soId")]
        public int? SalesOrderID { get; set; }

        [DataMember(Name = "soVersionId")]
        public int? SOVersionID { get; set; }

        [DataMember(Name = "soLineId")]
        public int? SOLineID { get; set; }

        [DataMember(Name = "poId")]
        public int PurchaseOrderID { get; set; }

        [DataMember(Name = "poVersionId")]
        public int POVersionID { get; set; }

        [DataMember(Name = "lineNum")]
        public string LineNum { get; set; }

        [DataMember(Name = "buyers")]
        public string Buyers { get; set; }

        [DataMember(Name = "conditionName")]
        public string ConditionName { get; set; }

        [DataMember(Name = "inTransit")]
        public bool InTransit { get; set; }

        [DataMember(Name = "isInspection")]
        public bool IsInspection { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "warehouseName")]
        public string WarehouseName { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

        [DataMember(Name = "allocated")]
        public List<AllocationsResponse> Allocated { get; set; }


    }

    [DataContract]
    public class OrderFulfillmentAllocationsResponse
    {
        [DataMember(Name = "orderNo")]
        public int OrderNo { get; set; }

        [DataMember(Name = "soLineId")]
        public int SOLineID { get; set; }

        [DataMember(Name = "lineNo")]
        public int LineNo { get; set; }

        [DataMember(Name = "customer")]
        public string Customer { get; set; }

        [DataMember(Name = "salesPerson")]
        public string Salesperson { get; set; }

        [DataMember(Name = "orderQty")]
        public int OrderQty { get; set; }

        [DataMember(Name = "resv")]
        public int Resv { get; set; }

        [DataMember(Name = "orderDate")]
        public string OrderDate { get; set; }
    }

    public class OrderFulfillmentAllocationsJSON
    {
        [JsonProperty("SOLineID")]
        public int soLineId { get; set; }

        [JsonProperty("SalesOrderID")]
        public int soId { get; set; }

        [JsonProperty("SOVersionID")]
        public int soVersionId { get; set; }

        [JsonProperty("LineNum")]
        public int lineNum { get; set; }

        [JsonProperty("AccountID")]
        public int accountId { get; set; }

        [JsonProperty("AccountName")]
        public string accountName { get; set; }

        [JsonProperty("OrderQty")]
        public int orderQty { get; set; }

        [JsonProperty("ResvQty")]
        public int resvQty { get; set; }
    }
}
