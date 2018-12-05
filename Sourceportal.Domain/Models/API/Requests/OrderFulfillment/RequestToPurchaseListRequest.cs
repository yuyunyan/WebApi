using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.OrderFulfillment
{
    [DataContract]
    public class RequestToPurchaseListRequest
    {
        [DataMember(Name = "underallocatedOnly")]
        public bool UnderallocatedOnly { get; set; }

        [DataMember(Name = "accountId")]
        public int? AccountId { get; set; }

        [DataMember(Name = "buyerId")]
        public int BuyerId { get; set; }

        [DataMember(Name = "rowOffset")]
        public int RowOffset { get; set; }

        [DataMember(Name = "rowLimit")]
        public int RowLimit { get; set; }

        [DataMember(Name = "sortBy")]
        public string SortBy { get; set; }

        [DataMember(Name = "descSort")]
        public bool DescSort { get; set; }
    }
}
