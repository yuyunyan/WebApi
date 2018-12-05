using System;

namespace Sourceportal.Domain.Models.DB.BOMs
{
    public class CustomerRFQLineBom
    {
        public int QuoteID { get; set; }
        public DateTime QuoteDate { get; set; }
        public string Customer { get; set; }
        public string Contact { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public int Mfg { get; set; }
        public int Qty { get; set; }
        public float TargetPrice { get; set; }
        public string Owners { get; set; }
        public string CustomerPartNum { get; set; }
        public string DateCode { get; set; }
        public decimal PriceDelta { get; set; }
        public decimal Potential { get; set; }
        public string BOMPartNumber { get; set; }
        public string BOMIntPartNumber { get; set; }
        public string BOMMfg { get; set; }
        public int BOMQty { get; set; }
        public decimal BOMPrice { get; set; }
        public int TotalRows { get; set; }
        public int ItemId { get; set; }
    }
}
