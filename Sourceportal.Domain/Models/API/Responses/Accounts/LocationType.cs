using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class LocationType
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }
}