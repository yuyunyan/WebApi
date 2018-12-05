using System;

namespace Sourceportal.Domain.Models.DB.BOMs
{
    public class EMSLineBom
    {
        public int ItemListID { get; set; }
        public DateTime BOMDate { get; set; }
        public string Customer { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public int Qty { get; set; }
        public decimal TargetPrice { get; set; }
        public string OwnerName { get; set; }
        public string CustomerPartNum { get; set; }
        public decimal PriceDelta { get; set; }
        public decimal Potential { get; set; }
        public int BOMQty { get; set; }
        public int BOMPrice { get; set; }
        public int TotalRows { get; set; }
        public int ItemId { get; set; }
        public string BomPartNumber { get; set; }
        public string BomIntPartNumber { get; set; }
        public string BomMfg { get; set; }
    }
}
