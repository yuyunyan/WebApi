using System;

namespace Sourceportal.Domain.Models.DB.BOMs
{
    public class VendorQuotesDbs
    {
        public int SourceID { get; set; }
        public DateTime Created { get; set; }
        public string AccountName { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public int Qty { get; set; }
        public decimal Cost { get; set; }
        public string Buyer { get; set; }
        public string DateCode { get; set; }
        public int LeadTimeDays { get; set; }
        public int TotalRows { get; set; }
        public int ItemId { get; set; }
        public string Note { get; set; }
        public int Spq { get; set; }
        public int Moq { get; set; }
        public int BomQty { get; set; }
        public decimal PriceDelta { get; set; }
        public decimal Potential { get; set; }
        public decimal BomPrice { get; set; }
        public string BomPartNumber { get; set; }
        public string BomIntPartNumber { get; set; }
        public string BomMfg { get; set; }

    }
}
