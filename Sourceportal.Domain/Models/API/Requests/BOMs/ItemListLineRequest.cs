using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.BOMs
{
    public class ItemListLineBOMRequest
    {
        public string PartNumber { get; set; }
        public string PartNumberStrip { get; set; }
        public string Manufacturer { get; set; }
        public string CustomerPartNumber { get; set; }
        public int? Qty { get; set; }
        public float? TargetPrice { get; set; }
        public string TargetDateCode { get; set; }
        public string AssocAccountID { get; set; }
        public int? QuoteTypeId { get; set; }
    }

    public class ItemListLineExcessRequest
    {
        public string PartNumber { get; set; }
        public string PartNumberStrip { get; set; }
        public string Manufacturer { get; set; }
        public string CustomerPartNumber { get; set; }
        public int? Qty { get; set; }
        public float? TargetPrice { get; set; }
        public string DateCode { get; set; }
        public int? MOQ { get; set; }
        public int? SPQ { get; set; }
    }
}
