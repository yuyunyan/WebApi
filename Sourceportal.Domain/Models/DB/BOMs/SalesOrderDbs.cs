using System;

namespace Sourceportal.Domain.Models.DB.BOMs
{
   public class SalesOrderDbs
    {
        public string Mfg { get; set; }
        public int SalesOrderId { get; set; }
        public int RecordId { get; set; }
        public DateTime SoDate { get; set; }
        public string Customer { get; set; }
        public string PartNumber { get; set; }
        public int QtySold { get; set; }
        public decimal SoldPrice { get; set; }
        public string DateCode { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime DueDate { get; set; }
        public int ShippedQty { get; set; }
        public string OrderStatus { get; set; }
        public decimal GrossProfitTotal { get; set;}
        public string SalesPerson { get; set; }
        public int ItemId { get; set; }
        public int BomQty { get; set; }
        public decimal PriceDelta { get; set; }
        public decimal Potential { get; set; }
        public decimal BomPrice { get; set; }
        public string BomPartNumber { get; set; }
        public string BomIntPartNumber { get; set; }
        public string BomMfg { get; set; }
        public int TotalRows { get; set; }
    }
}
