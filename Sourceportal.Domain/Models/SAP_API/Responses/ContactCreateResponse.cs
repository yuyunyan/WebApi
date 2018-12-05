using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.SAP_API.Responses
{
    public class ContactCreateResponse : SapApiBaseResonse
    {
        [DataMember(Name = "contactExternalId")]
        public string ContactExternalId { get; set; }
    }
}
