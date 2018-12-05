using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    [DataContract]
    public class SourcingQuoteLinesListResponse : BaseResponse
    {
        [DataMember(Name = "quoteLines")]
        public List<SourcingQuoteLinesResponse> SourcingQuoteLinesList;
        [DataMember(Name = "totalRows")]
        public int TotalRows;
    }
    public class SourcingQuoteLinesResponse
    {
        [DataMember(Name = "quoteLineId")]
        public int QuoteLineID { get; set; }

        [DataMember(Name = "quoteId")]
        public int QuoteID { get; set; }

        [DataMember(Name = "quoteVersionId")]
        public int QuoteVersionID { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "partNumberStrip")]
        public string PartNumberStrip { get; set; }

        [DataMember(Name = "mfg")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityID { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusID { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingID { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }

        [DataMember(Name = "sourcesCount")]
        public int SourcesCount { get; set; }

        [DataMember(Name = "rfqCount")]
        public int RFQCount { get; set; }
        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }
        [DataMember(Name = "price")]
        public decimal Price { get; set; }
        [DataMember(Name = "itemID")]
        public int ItemID { get; set; }
        [DataMember(Name = "itemListLineId")]
        public int ItemListLineID { get; set; }
        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }
        [DataMember(Name = "shipDate")]
        public string ShipDate { get; set; }
        [DataMember(Name = "customerLine")]
        public int CustomerLine { get; set; }
        [DataMember(Name = "customerPartNumber")]
        public string CustomerPartNumber { get; set; }
        [DataMember(Name = "quoteTypeName")]
        public string QuoteTypeName { get; set; }
        [DataMember(Name = "owners")]
        public string Owners { get; set; }
    }
}