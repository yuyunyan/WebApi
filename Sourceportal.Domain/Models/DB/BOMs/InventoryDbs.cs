namespace Sourceportal.Domain.Models.DB.BOMs
{
    public class InventoryDbs
    {
        public int POLineID { get; set; }
        public int ItemID { get; set; }
        public string Manufacturer { get; set; }
        public string WarehouseCode { get; set; }
        public int InventoryQty { get; set; }
        public string PartNumber { get; set; }
        public decimal Cost { get; set; }
        public int ReservedQty { get; set; }
        public int AvailableQty { get; set; }
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
