using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.PurchaseOrders
{
    public class PurchaseOrderLinesDb
    {
        public int POLineId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int StatusIsCanceled { get; set; }
        public int LineNum { get; set; }
        public int LineRev { get; set; }
        public int VendorLine { get; set; }
        public int ItemId { get; set; }
        public string PartNumber { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int Qty { get; set; }
        public double Cost { get; set; }
        public string DateCode { get; set; }
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public int PackageConditionID { get; set; }
        public string ConditionName { get; set; }
        public string MfrName { get; set; }
        public string Error { get; set; }
        public int TotalRows { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PromisedDate { get; set; }
        public int Comments { get; set; }
        public bool IsSpecBuy { get; set; }
        public int SpecBuyForUserId { get; set; }
        public int SpecBuyForAccountID { get; set; }
        public string SpecBuyReason { get; set; }
        public int AllocatedQty { get; set; }
        public int PurchaseOrderID { get; set; }
        public int POVersionID { get; set; }
        public bool IsDeleted { get; set; }
        public int? SalesOrderID { get; set; }
        public int? SOVersionID { get; set; }
        public int? ClonedFromID { get; set; }
        public string ExternalID { get; set; }
        public int ToWarehouseID { get; set; }
    }
}
