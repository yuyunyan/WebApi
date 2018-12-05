namespace Sourceportal.Domain.Models.DB.RFQ
{
    public class RfqLinesDb
    {
        public int VRFQLineID { get; set; }
        public int LineNum { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public int Qty { get; set; }
        public float TargetCost { get; set; }
        public string DateCode { get; set; }
        public int PackagingID { get; set; }
        public string PackagingName { get; set; }
        public string Note { get; set; }
        public int SourcesTotalQty { get; set; }
        public int StatusID { get; set; }
        public int ItemID { get; set; }
        public string PartNumberStrip { get; set; }
        public int HasNoStock { get; set; }
        public string ErrorMessage { get; set; }
        public int RowCount { get; set; }
        public string AccountName { get; set; }
        public string ContactName { get; set; }
        public string SentDate { get; set; }
        public int Age { get; set; }
        public string OwnerName { get; set; }
        public int VendorRFQID { get; set; }
        public int AccountID { get; set; }
        public int ContactID { get; set; }
    }
}
