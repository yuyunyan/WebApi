using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.PurchaseOrders
{
    public class SetPurchaseOrderExtraRequest
    {
        public int POExtraId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int POVersionId { get; set; }
        public int StatusId { get; set; }
        public int ItemExtraId { get; set; }
        public int LineNum { get; set; }
        public int RefLineNum { get; set; }
        public int Qty { get; set; }
        public decimal Cost { get; set; }
        public int PrintOnPO { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    }
}
