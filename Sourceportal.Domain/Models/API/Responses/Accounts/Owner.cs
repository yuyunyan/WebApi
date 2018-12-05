using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class Owner
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public string UserId { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

        [DataMember(Name = "ownerImageUrl")]
        public string OwnerImageURL { get; set; }

        [DataMember(Name = "percentage")]
        public decimal Percentage { get; set; }
    }
}