using System.Runtime.Serialization;
using Sourceportal.Domain.Models.API.Requests.Accounts;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class NewAccountDetails : AccountBasicDetails
    {
        [DataMember(Name = "location")]
        public Location Location { get; set; }

        [DataMember(Name = "contact")]
        public SetContactDetailsRequest Contact { get; set; }
    }
}
