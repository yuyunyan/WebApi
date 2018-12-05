using System;

namespace Sourceportal.Domain.Models.DB.BOMs
{
    public class PurchaseOrderLineBom
    {
        public DateTime PODate { get; set; }
        public string Vendor { get; set; }
        public string MfgPartNumber { get; set; }
        public string Mfg { get; set; }
        public int QtyOrdered { get; set; }
        public decimal POCost { get; set; }
        public string Buyer { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int ReceivedQty { get; set; }
        public string OrderStatus { get; set; }
        public int PurchaseOrderID { get; set; }
        public int POLineID { get; set; }
        public string DateCode { get; set; }
        public int TotalRows { get; set; }
        public int ItemId { get; set; }
        public int BomQty { get; set; }
        public decimal PriceDelta { get; set; }
        public decimal Potential { get; set; }
        public decimal BomPrice { get; set; }
        public string BomPartNumber { get; set; }
        public string BomIntPartNumber { get; set; }
        public string BomMfg { get; set; }

    }
}
