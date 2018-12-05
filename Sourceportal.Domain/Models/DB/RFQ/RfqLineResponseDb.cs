namespace Sourceportal.Domain.Models.DB.RFQ
{
    public class RfqLineResponseDb
    {
        public int SourceId { get; set; }
        public int ItemID { get; set; }
        public int LineNum { get; set; }
        public int OfferQty { get; set; }
        public decimal Cost { get; set; }
        public string DateCode { get; set; }
        public int Moq { get; set; }
        public int Spq { get; set; }
        public int LeadTimeDays { get; set; }
        public double ValidForHours { get; set; }
        public int PackagingId { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public string PackagingName { get; set; }
        public int RowCount { get; set; }
        public string ErrorMessage { get; set; }
        public int Comments { get; set; }
        public bool IsNoStock { get; set; }
    }
}
