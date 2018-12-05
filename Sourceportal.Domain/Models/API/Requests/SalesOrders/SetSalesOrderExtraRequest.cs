using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.SalesOrders
{
    public class SetSalesOrderExtraRequest
    {
        public int SOExtraId { get; set; }
        public int SalesOrderId { get; set; }
        public int QuoteExtraId { get; set; }
        public int SOVersionId { get; set; }
        public int StatusId { get; set; }
        public int ItemExtraId { get; set; }
        public int LineNum { get; set; }
        public int RefLineNum { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int PrintOnSO { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    }
}
