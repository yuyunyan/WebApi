using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.SalesOrders
{
    [DataContract]
    public class SetExternalIdRequest
    {
        [DataMember(Name= "objectId")]
        public int ObjectId { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }
}
