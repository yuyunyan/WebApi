using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    [DataContract]
  public class SourceHistoryResponse
    {
        [DataMember(Name = "sourceLineList")]
        public List<SourceLineResponse> SourceLineList;

    }

    [DataContract]
    public class SourceLineResponse
    {
        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "date")]
        public DateTime Created { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "datecode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packaging")]
        public string Packaging { get; set; }

        [DataMember(Name = "leadTimeDays")]
        public int LeadTimeDays { get; set; }

        [DataMember(Name = "owners")]
        public string Owners { get; set; }

        [DataMember(Name = "contactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }
    }
}
