using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.Middleware
{
    [DataContract]
    public class MiddlewareSyncBase
    {
        public MiddlewareSyncBase(int id, string externalId)
        {
            Id = id;
            ExternalId = externalId;
        }

        [DataMember(Name = "id")]
        public int Id { get; private set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; private set; }
        
    }
}
