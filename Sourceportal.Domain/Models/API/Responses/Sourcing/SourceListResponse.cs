using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    [DataContract]
    public class SourceListResponse
    {
        [DataMember(Name = "sourceResponse")]
        public List<SourceResposne> SourceResponse;
    }

    [DataContract]
    public class SourceResposne
    {
        [DataMember(Name = "sourceId")]
        public int SourceId { get; set; }

        [DataMember(Name = "sourceTypeId")]
        public string SourceTypeId { get; set; }

        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }

        [DataMember(Name = "typeRank")]
        public int TypeRank { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "supplier")]
        public string Supplier { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "contactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityId { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingId { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "packagingConditionId")]
        public int PackagingConditionID { get; set; }

        [DataMember(Name = "moq")]
        public int MOQ { get; set; }

        [DataMember(Name = "spq")]
        public int SPQ { get; set; }

        [DataMember(Name = "leadTimeDays")]
        public int LeadTimeDays { get; set; }

        [DataMember(Name = "validForHours")]
        public int ValidForHours { get; set; }

        [DataMember(Name = "activity")]
        public Activity Activity { get; set; }

        [DataMember(Name = "isMatched")]
        public bool? IsMatched { get; set; }

        [DataMember(Name = "isJoined")]
        public bool IsJoined { get; set; }

        [DataMember(Name = "showCheckmark")]
        public bool showCheckmark { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }
        [DataMember(Name = "ageInDays")]
        public int AgeInDays { get; set; }
        [DataMember(Name = "created")]
        public string Created { get; set; }
        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }
        [DataMember(Name = "rtpQty")]
        public int? RTPQty { get; set; }

        [DataMember(Name = "rating")]
        public string Rating { get; set; }
        [DataMember(Name = "buyerId")]
        public int BuyerID { get; set; }
    }

    public class Activity
    {
        [DataMember(Name = "objectTypeId")]
        public string ObjectTypeId { get; set; }
        [DataMember(Name = "activity")]
        public string Count { get; set; }
    }
}
