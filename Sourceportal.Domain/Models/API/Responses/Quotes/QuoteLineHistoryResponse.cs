using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
   public class QuoteLineHistoryResponse
    {
        [DataMember(Name = "quoteLineList")]
        public List<QuoteLineResponse> QuoteLineList;
    }

    [DataContract]
    public class QuoteLineResponse
    {
        [DataMember(Name = "quoteLineId")]
        public int QuoteLineId { get; set; }

        [DataMember(Name = "quoteId")]
        public int QuoteId { get; set; }

        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "contactFirstName")]
        public string ContactFirstName { get; set; }

        [DataMember(Name = "contactLastName")]
        public string ContactLastName { get; set; }

        [DataMember(Name = "contactFullName")]
        public string ContactFullName { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "gpm")]
        public decimal GPM { get; set; }

        [DataMember(Name = "datecode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packaging")]
        public string Packaging { get; set; }

        [DataMember(Name = "quoteDate")]
        public DateTime? QuoteDate { get; set; }

        [DataMember(Name = "owners")]
        public string Owners { get; set; }


    }
}
