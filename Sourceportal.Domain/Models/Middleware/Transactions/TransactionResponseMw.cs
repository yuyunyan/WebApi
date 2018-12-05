using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Middleware.Transactions
{
    [DataContract]
    public class TransactionResponseMw
    {
        [DataMember(Name = "transactions")]
        public List<TransactionMw> Transactions { get; set; }
    }

    [DataContract]
    public class TransactionMw
    {
        [DataMember(Name = "transactionId")]
        public string TransactionId { get; set; }

        [DataMember(Name = "objectId")]
        public int ObjectId { get; set; }

        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "direction")]
        public string Direction { get; set; }

        [DataMember(Name = "objectType")]
        public string ObjectType { get; set; }

        [DataMember(Name = "creatorId")]
        public int CreatorId { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }

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
