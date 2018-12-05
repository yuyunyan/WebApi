using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class InspectionGridResponse
    {
        [DataMember(Name = "inspectionList")]
        public IList<InspectionGridItem> InspectionList { get; set; }
        
        [DataMember(Name = "rowCount")]
        public int RowCount { get; set; }
    }

    [DataContract]
    public class InspectionGridItem
    {
        [DataMember(Name = "inspectionId")]
        public int InspectionId { get; set; }

        [DataMember(Name = "iItemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "supplier")]
        public string Supplier { get; set; }

        [DataMember(Name = "poNumber")]
        public string PoNumber { get; set; }

        [DataMember(Name = "customers")]
        public List<InspectionCustomersJsonResponse> Customers { get; set; }

        [DataMember(Name = "salesOrders")]
        public List<InspectionSalesOrdersResponse> SalesOrders { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "inspectionTypeId")]
        public int InspectionTypeID { get; set; }

        [DataMember(Name = "inspectionTypeName")]
        public string InspectionTypeName { get; set; }

        [DataMember(Name = "inventoryId")]
        public int InventoryID { get; set; }

        [DataMember(Name = "stockExternalId")]
        public string StockExternalID { get; set; }

        [DataMember(Name = "receivedDate")]
        public string ReceivedDate { get; set; }

        [DataMember(Name = "shipDate")]
        public string ShipDate { get; set; }

        [DataMember(Name = "lotNumber")]
        public int LotNumber { get; set; }

        [DataMember(Name = "warehouseId")]
        public int WarehouseID { get; set; }

        [DataMember(Name = "warehouseName")]
        public string WarehouseName { get; set; }

        [DataMember(Name = "poVersionID")]
        public string POVersionID;

        [DataMember(Name = "accountId")]
        public int AccountID;

        [DataMember(Name = "poExternalId")]
        public string POExternalID;
    }

    [DataContract]
    public class InspectionSalesOrdersResponse
    {
        [DataMember(Name = "salesOrderID")]
        public int SalesOrderID { get; set; }

        [DataMember(Name = "externalID")]
        public int ExternalID { get; set; }
    }

    [DataContract]
    public class InspectionCustomersJsonResponse
    {
        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }
    }

}
