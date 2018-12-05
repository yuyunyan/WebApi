using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.SAP_API.Responses
{
    public class SapAccountCreateResponse : SapApiBaseResonse
    {
        [DataMember(Name = "accountExternalId")]
        public string AccountId { get; set; }

        [DataMember(Name = "locationExternalId")]
        public string LocationExternalId { get; set; }

        [DataMember(Name = "contactExternalId")]
        public string ContactExternalId { get; set; }
        
    }
}
