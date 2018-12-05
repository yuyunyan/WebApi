using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.ItemStock
{
    public class ItemStockDB
    {
        public int InspectionID { get; set; }
        public int ItemStockID { get; set; }
        public int POLineID { get; set; }
        public int ItemID { get; set; }
        public string MfrLotNum { get; set; }
        public int Qty { get; set; }
        public int WarehouseID { get; set; }
        public int WarehouseBinID { get; set; }
        public int InvStatusID { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string DateCode { get; set; }
        public int? PackagingID { get; set; }
        public int? PackageConditionID { get; set; }
        public int InspectionWarehouseID { get; set; }
        public bool IsRejected { get; set; }
        public int? COO { get; set; }
        public DateTime? Expiry { get; set; }
        public string StockDescription { get; set; }
        public string ExternalID { get; set; }
        public bool IsDeleted { get; set; }
        public int ClonedFromID { get; set; }
        public int AcceptedBinID { get; set; }
        public string AcceptedBinName { get; set; }
        public int RejectedBinID { get; set; }
        public string RejectedBinName { get; set; }
    }
}
