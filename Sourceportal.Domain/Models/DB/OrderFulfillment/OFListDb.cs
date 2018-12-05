namespace Sourceportal.Domain.Models.DB.OrderFulfillment
{
   public class OFListDb
    {
        public int SoLineId { get; set; }
        public int SalesOrderId { get; set; }
        public int VersionID { get; set; }
        public int? ItemId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string PartNumber { get; set; }
        public string MfrId { get; set; }
        public string MfrName { get; set; }
        public int Qty { get; set; }
        public int Remaining { get; set; }
        public double Price { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public string DateCode { get; set; }
        public string ShipDate { get; set; }
        public string Owner { get; set; }
        public string Buyers { get; set; }
        public int TotalRows { get; set; }
        public int TotalRowCount { get; set; }
        public int Comments { get; set; }
        public int LineNum { get; set; }
        public int AllocatedQty { get; set; }
        public string DueDate { get; set; }
        public double Cost { get; set; }
        public string ExternalID { get; set; }
    }
}
