using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.SalesOrders
{
    public class SalesOrderLinesDb
    {
        public int SOLineId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int StatusIsCanceled { get; set; }
        public int LineNum { get; set; }
        public int ItemId { get; set; }
        public int CustomerLine { get; set; }
        public string PartNumber { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public string CustomerPartNum { get; set; }
        public int Qty { get; set; }
        public int? DeliveryRuleId { get; set; }
        public int Reserved { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string DateCode { get; set; }
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public int PackageConditionID { get; set; }
        public DateTime ShipDate { get; set; }
        public string MfrName { get; set; }
        public string Error { get; set; }
        public int TotalRows { get; set; }
        public DateTime DueDate { get; set; }
        public int Comments { get; set; }
        public string ProductSpec { get; set; }
    }
}
