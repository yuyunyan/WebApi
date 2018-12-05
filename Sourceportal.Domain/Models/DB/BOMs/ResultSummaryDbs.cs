using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.BOMs
{
   public class ResultSummaryDbs
    {
        public int ItemId { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public int SalersOrders { get; set; }
        public int Inventory { get; set; }
        public int PurchaseOrders { get; set; }
        public int VendorQuotes { get; set; }
        public int CustomerQuotes { get; set; }
        public int CustomerRfq { get; set; }
        public int OutsideOffers { get; set; }
        public int Bom { get; set; }
        public int RowCount { get; set; }

    }
}
