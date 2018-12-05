using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Sync.SynchronousResponses
{
    [DataContract]
    public class PoAllocateSyncResponse : BaseResponse
    {
        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }
}
