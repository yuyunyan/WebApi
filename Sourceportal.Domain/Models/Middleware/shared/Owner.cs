namespace Sourceportal.Domain.Models.Middleware
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Owner
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
