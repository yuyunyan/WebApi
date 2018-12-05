using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Transactions
{
    [DataContract]
    public class TransactionsResponse
    {
        [DataMember(Name = "transactions")]
        public List<TransactionDetailResponse> Transactions { get; set; }
    }

    [DataContract]
    public class TransactionDetailResponse
    {
        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "direction")]
        public string Direction { get; set; }

        [DataMember(Name = "objectType")]
        public string ObjectType { get; set; }

        [DataMember(Name = "creator")]
        public string Creator { get; set; }

        [DataMember(Name = "owners")]
        public string Owners { get; set; }

        [DataMember(Name = "errors")]
        public List<string> Errors { get; set; }

        [DataMember(Name = "payload")]
        public string Payload { get; set; }

    }

}
