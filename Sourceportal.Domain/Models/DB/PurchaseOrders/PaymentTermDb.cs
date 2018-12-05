using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.PurchaseOrders
{
    public class PaymentTermDb
    {
        public int PaymentTermID { get; set; }
        public string TermName { get; set; }
        public string TermDesc { get; set; }
        public string NetDueDays { get; set; }
        public string DiscountDays { get; set; }
        public double DiscountPercent { get; set; }
        public string ExternalID { get; set; }

    }
}
