using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemQuotesResponse
    {
        [DataMember(Name = "items")]
        public List<ItemQuotesDetails> ItemQuotes{ get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }

    [DataContract]
    public class ItemQuotesDetails
    {
        [DataMember(Name = "quoteId")]
        public int QuoteID { get; set; }

        [DataMember(Name = "quoteLineId")]
        public int QuoteLineID { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionID { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "createdDate")]
        public string Created { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactID { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "organizationid")]
        public int OrganizationID { get; set; }

        [DataMember(Name = "orgName")]
        public string OrgName { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusID { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "sentDate")]
        public string SentDate { get; set; }

        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }

        [DataMember(Name = "cost")]
        public string Cost { get; set; }

        [DataMember(Name = "price")]
        public string Price { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingID { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "gpm")]
        public string GPM { get; set; }

        [DataMember(Name = "owners")]
        public string Owners { get; set; }

    }
}
