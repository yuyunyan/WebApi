using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.Middleware.Owners
{
    [DataContract]
    public class SyncOwnership
    {
        [DataMember(Name = "leadOwner")]
        public SyncOwner LeadOwner { get; set; }

        [DataMember(Name = "secondOwner")]
        public SyncOwner SecondOwner { get; set; }
    }
}
