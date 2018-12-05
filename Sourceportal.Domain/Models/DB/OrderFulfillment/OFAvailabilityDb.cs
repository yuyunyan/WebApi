using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.OrderFulfillment
{
    public class OFAvailabilityDb
    {
        public string Type { get; set; }

        public int ID { get; set; }

        public int ItemID { get; set; }

        public string WarehouseName { get; set; }

        public string PartNumber { get; set; }

        public int MfrID { get; set; }

        public string MfrName { get; set; }

        public int CommodityID { get; set; }

        public string CommodityName { get; set; }

        public int AccountID { get; set; }

        public string AccountName { get; set; }

        public int StatusID { get; set; }

        public string StatusName { get; set; }

        public int OrigQty { get; set; }

        public int Available { get; set; }

        public decimal Cost { get; set; }

        public string DateCode { get; set; }

        public int PackagingID { get; set; }

        public string PackagingName { get; set; }

        public string PromisedDate { get; set; }

        public int? SalesOrderID { get; set; }

        public int? SOVersionID { get; set; }

        public int? SOLineID { get; set; }

        public int PurchaseOrderID { get; set; }

        public int POVersionID { get; set; }

        public int LineNum { get; set; }

        public int LineRev { get; set; }

        public string Buyers { get; set; }

        public DateTime ShipDate { get; set; }

        public string ConditionName { get; set; }

        public bool InTransit { get; set; }

        public bool IsInspection { get; set; }

        public int Comments { get; set; }

        public int TotalRows { get; set; }

        public string ExternalID { get; set; }

        public string Allocations { get; set; }
    }
}
