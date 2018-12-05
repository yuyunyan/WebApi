using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class CompanyType
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }
}