using System.Runtime.Serialization;
using Sourceportal.Domain.Models.API.Responses.Accounts;

namespace Sourceportal.Domain.Models.SAP_API.Requests
{
    [DataContract]
    public class CreateAccountSapRequest
    {
        [DataMember(Name = "accountDetails")]
        public AccountBasicDetails AccountDetails { get; set; }

        [DataMember(Name = "locationDetails")]
        public Location LocationDetails { get; set; }
    }
}
