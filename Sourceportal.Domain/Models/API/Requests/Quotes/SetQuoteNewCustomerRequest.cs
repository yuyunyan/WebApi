using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Responses.Accounts;

namespace Sourceportal.Domain.Models.API.Requests.Quotes
{
    [DataContract]
    public class SetQuoteNewCustomerRequest
    {
        [DataMember(Name = "organizationId")]
        public int OrganizationID { get; set; }

        [DataMember(Name = "accountDetails")]
        public AccountBasicDetails AccountDetails { get; set; }

        [DataMember(Name = "contactDetails")]
        public SetContactDetailsRequest ContactDetails { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "quoteParts")]
        public List<SetPartsListRequest> QuoteParts { get; set; }
    }
}
