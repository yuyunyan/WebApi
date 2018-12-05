using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.OrderFulfillment
{
    public class OFGridExportLine
    {
        public int OrderNumber { get; set; }
        public int LineNumber { get; set; }
        public string Customer { get; set; }
        public string PartNumber { get; set; }
        public string Mfr { get; set; }
        public string Commodity { get; set; }
        public int OrderQty { get; set; }
        public int AllocatedQty { get; set; }
        public double Price { get; set; }
        public string Package { get; set; }
        public string DateCode { get; set; }
        public string ShipDate { get; set; }
        public string SalesPerson { get; set; }
        public string Buyer { get; set; }
    }
}
