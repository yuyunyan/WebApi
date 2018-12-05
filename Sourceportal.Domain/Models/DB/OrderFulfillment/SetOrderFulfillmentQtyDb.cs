using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.OrderFulfillment
{
    public class SetOrderFulfillmentQtyDb
    {
        public int SOLineID { get; set; }
        public int POLineID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int POVersionID { get; set; }
        public int InventoryID { get; set; }
        public int Qty { get; set; }
    }
}
