using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.Middleware.Owners
{
    [DataContract]
    public class SyncOwner
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }

        [DataMember(Name = "percentage")]
        public decimal Percentage { get; set; }
    }
}
