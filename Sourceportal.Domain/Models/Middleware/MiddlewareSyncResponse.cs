using System;
using System.Runtime.Serialization;
using Sourceportal.Domain.Models.API.Responses;

namespace Sourceportal.Domain.Models.Middleware
{
    public class MiddlewareSyncResponse : BaseResponse
    {
        [DataMember(Name = "transactionId")]
        public string TransactionId { get; set; }

        [DataMember(Name = "objectId")]
        public int ObjectId { get; set; }

        [DataMember(Name = "objectType")]
        public string ObjectType { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}
