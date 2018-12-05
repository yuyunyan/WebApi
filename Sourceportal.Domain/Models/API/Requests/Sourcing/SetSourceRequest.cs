using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Sourcing
{
    [DataContract]
    public class SetSourceRequest
    {
        [DataMember(Name = "sourceId")]
        public int SourceId { get; set; }

        [DataMember(Name = "sourceTypeId")]
        public string SourceTypeId { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityId { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingId { get; set; }

        [DataMember(Name = "moq")]
        public int MOQ { get; set; }

        [DataMember(Name = "spq")]
        public int SPQ { get; set; }

        [DataMember(Name = "leadTimeDays")]
        public int LeadTimeDays { get; set; }

        [DataMember(Name = "count")]
        public string Count { get; set; }

        [DataMember(Name = "currencyId")]
        public string CurrencyId { get; set; }

        [DataMember(Name = "validForHours")]
        public int ValidForHours { get; set; }

        [DataMember(Name = "requestToBuy")]
        public int RequestToBuy { get; set; }

        [DataMember(Name = "rtbQty")]
        public int RtbQty { get; set; }

        [DataMember(Name = "isDeleted")]
        public int IsDeleted { get; set; }

        [DataMember(Name = "isIhsItem")]
        public bool IsIhsItem { get; set; }


    }
}
