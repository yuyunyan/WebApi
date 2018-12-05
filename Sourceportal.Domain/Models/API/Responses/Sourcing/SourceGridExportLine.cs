using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    public class SourceGridExportLine
    {
        public string Type { get; set; }
        public string Date { get; set; }
        public string PartNumber { get; set; }
        public string Mfr { get; set; }
        public string Buyer { get; set; }
        public string Commodity { get; set; }
        public string Supplier { get; set; }
        public string Rating { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Packaging { get; set; }
        public string DateCode { get; set; }
        public int LeadTimeDays { get; set; }
        public int? MOQ { get; set; }
    }
}
