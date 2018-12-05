using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.SalesOrders
{
    public class SetSalesOrderLinesRequest
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
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string DateCode { get; set; }
        public int PackagingId { get; set; }
        public string PackingName { get; set; }
        public DateTime ShipDate { get; set; }
        public string MfrName { get; set; }
        public int SalesOrderId { get; set; }
        public int SOVersionId { get; set; }

    }
}
