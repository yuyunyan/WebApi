using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Sync
{
    [DataContract]
    public class SyncResponse : BaseResponse
    {
        [DataMember(Name = "transactionId")]
        public string TransactionId { get; set; }
    }
}
