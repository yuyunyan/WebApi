using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.Accounts;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
    public class QuoteDetailsResponse
    {
        [DataMember(Name = "QuoteId")]
        public int QuoteId { get; set; }
        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }
        [DataMember(Name = "customerId")]
        public int AccountId { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "officePhone")]
        public string OfficePhone { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "shipLocationId")]
        public int ShipLocationId { get; set; }

        [DataMember(Name = "validDays")]
        public double ValidDays { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }

        [DataMember(Name = "organizationId")]
        public int OrganizationId { get; set; }

        [DataMember(Name = "currencyId")]
        public string CurrencyId { get; set; }

        [DataMember(Name = "incotermId")]
        public int IncotermId { get; set; }

        [DataMember(Name = "paymentTermId")]
        public int PaymentTermId { get; set; }

        [DataMember(Name = "shippingMethodId")]
        public int ShippingMethodId { get; set; }

        [DataMember(Name = "quoteTypeId")]
        public int QuoteTypeId { get; set; }

        [DataMember(Name = "ownerList")]
        public List<Owner> OwnerList { get; set; }

        [DataMember(Name = "incotermLocation")]
        public string IncotermLocation { get; set; }

    }

}
