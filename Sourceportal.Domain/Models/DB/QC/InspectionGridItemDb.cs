using System;

namespace Sourceportal.Domain.Models.DB.QC
{
    public class InspectionGridItemDb
    {
        public int InspectionId;
        public int ItemId;
        public string Supplier;
        public string PoNumber;
        public string POVersionID;
        public string Customers;
        public string StatusName;
        public int InspectionTypeID;
        public string InspectionTypeName;
        public int InventoryID;
        public string StockExternalID;
        public DateTime? ReceivedDate;
        public DateTime? ShipDate;
        public int RowCount;
        public int LotNumber;
        public int WarehouseID;
        public string WarehouseName;
        public int AccountID;
        public string POExternalID;
    }
}
