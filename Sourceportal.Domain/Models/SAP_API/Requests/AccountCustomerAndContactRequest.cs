using System.Runtime.Serialization;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Responses.Accounts;

namespace Sourceportal.Domain.Models.SAP_API.Requests
{
    [DataContract]
    public class AccountCustomerAndContactRequest
    {
        [DataMember(Name = "accountDetails")]
        public AccountBasicDetails AccountDetails { get; set; }

        [DataMember(Name = "contactDetails")]
        public SetContactDetailsRequest ContactDetails { get; set; }
    }
}
