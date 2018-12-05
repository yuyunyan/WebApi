using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.OrderFulfillment
{
    public class SOAllocationDb
    {
        public int SOLineID { get; set; }
        public int SalesOrderID { get; set; }
        public int SOVersionID { get; set; }
        public string AccountName { get; set; }
        public string StatusName { get; set; }
        public string PartNumber { get; set; }
        public string MfrName { get; set; }
        public int Qty { get; set; }
        public int Needed { get; set; }
        public int Allocated { get; set; }
        public decimal Price { get; set; }
        public int MyProperty { get; set; }
        public DateTime ShipDate { get; set; }
        public string DateCode { get; set; }
        public string Sellers { get; set; }
        public int LineNum { get; set; }
        public string ExternalID { get; set; }
    }
}
