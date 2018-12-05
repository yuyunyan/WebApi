using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.OrderFulfillment
{
    public class ItemInventoryDb
    {
        public int InventoryID { get; set; }
        public int StockID { get; set; }
        public int POLineID { get; set; }
        public int ItemID { get; set; }
        public int InvStatusID { get; set; }
        public int WarehouseBinID { get; set; }
        public string BinName { get; set; }
        public string WarehouseBinExternalId { get; set; }
        public string WarehouseBinExternalUUID { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseExternalId { get; set; }
        public int Qty { get; set; }
        public string DateCode { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int? PackagingID { get; set; }
        public int? PackageConditionID { get; set; }
        public string ExternalID { get; set; }
        public bool IsDeleted { get; set; }
        public bool StockDeleted { get; set; }
        public bool IsInspection { get; set; }
    }
}
