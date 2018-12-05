using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
    public class QuoteListResponse : BaseResponse
    {
        [DataMember(Name = "quotes")]
        public List<QuoteResponse> QuoteList;
        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }
    public class QuoteResponse
    {
        [DataMember(Name = "quoteId")]
        public int QuoteId { get; set; }

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

        [DataMember(Name = "organizationId")]
        public int OrganizationId { get; set; }

        [DataMember(Name = "sentDate")]
        public DateTime? SentDate { get; set; }
        [DataMember(Name = "Owners")]
        public string Owners { get; set; }
        [DataMember(Name = "countryName")]
        public string CountryName { get; set; }

    }
   

}
