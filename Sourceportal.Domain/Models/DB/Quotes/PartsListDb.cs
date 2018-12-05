using System;

namespace Sourceportal.Domain.Models.DB.Quotes
{
   public class PartsListDb
    {
        public int QuoteLineId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int StatusIsCanceled { get; set; }
        public int LineNum { get; set; }
        public int AltFor { get; set; }
        public int ItemId { get; set; }
        public string ItemExternalId { get; set; }
        public int CustomerLine { get; set; }
        public string PartNumber { get; set; }
        public string PartNumberStrip { get; set; }
        public string Manufacturer { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public string CustomerPartNum { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public double GPM { get; set; }
        public string PriceFormatted { get; set; }
        public string ExtPrice { get; set; }
        public string DateCode { get; set; }
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public int PackageConditionID { get; set; }
        public string ConditionName { get; set; }
        public DateTime ShipDate { get; set; }
        public string ShipDateFormatted { get; set; }
        public string RoutedTo { get; set; }
        public int CommentCount { get; set; }
        public bool IsRoutedToBuyers { get; set; }
        public string Error { get; set; }
        public int Comments { get; set; }
        public string PartNoMfgCombined { get; set; }
        public string MfgDateCodeCombined { get; set; }
        public bool IsPrinted { get; set; }
        public int SortBy { get; set; }
        public string HasSourceMatch { get; set; }
        public int SourceMatchCount { get; set; }
        public int SourceMatchQty { get; set; }
        public string SourceType { get; set; }
        public int? LeadTimeDays { get; set; }
    }
}
