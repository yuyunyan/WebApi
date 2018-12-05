using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.PurchaseOrders
{
    public class PurchaseOrderSOAllocation
    {
        public int POLineID { get; set; }
        public int SOLineID { get; set; }
        public int Qty { get; set; }
    }
}
