using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
   [DataContract]
   public class PartDetails : BaseResponse
    {
        [DataMember(Name = "quoteLineId")]
        public int QuoteLineId { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "statusCanceled")]
        public int StatusCanceled { get; set; }

        [DataMember(Name = "lineNo")]
        public int LineNo { get; set; }

        [DataMember(Name = "itemId")]
        public long ItemId { get; set; }

        [DataMember(Name = "customerLineNo")]
        public int CustomerLine { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "partNumberStrip")]
        public string PartNumberStrip { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityId { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "packageConditionId")]
        public int PackageConditionID { get; set; }

        [DataMember(Name = "conditionName")]
        public string ConditionName { get; set; }
        
        [DataMember(Name = "customerPartNo")]
        public string CustomerPartNo { get; set; }

        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "gpm")]
        public double GPM { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackingId { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackingName { get; set; }

        [DataMember(Name = "shipDate")]
        public string ShipDate { get; set; }
        [DataMember(Name = "alternateId")]
        public int AlternateId { get; set; }

        [DataMember(Name = "alternates")]
        public List<PartDetails> Alternates { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "quoteId")]
        public int QuoteId { get; set; }

        [DataMember(Name = "quoteVersionId")]
        public int QuoteVersionId { get; set; }

        [DataMember(Name = "altFor")]
        public int AltFor { get; set; }

        [DataMember(Name = "isRoutedToBuyers")]
        public bool IsRoutedToBuyers { get; set; }

        [DataMember(Name = "targetPrice")]
        public decimal TargetPrice { get; set; }

        [DataMember(Name = "targetDateCode")]
        public string TargetDateCode { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }

        [DataMember(Name = "routedTo")]
        public List<QuoteRouteToResponse> RoutedTo { get; set; }

        [DataMember(Name = "isIhs")]
        public bool IsIhs { get; set; }

        [DataMember(Name = "isPrinted")]
        public bool IsPrinted { get; set; }

        [DataMember(Name = "sourceMatchStatus")]
        public string SourceMatchStatus { get; set; }

        [DataMember(Name = "sourceMatchCount")]
        public int SourceMatchCount { get; set; }

        [DataMember(Name = "sourceMatchQty")]
        public int SourceMatchQty { get; set; }

        [DataMember(Name = "sourceType")]
        public string SourceType { get; set; }

        [DataMember(Name = "leadTimeDays")]
        public int? LeadTimeDays { get; set; }

        [DataMember(Name = "sourceId")]
        public int? SourceID { get; set; }

    }
}
