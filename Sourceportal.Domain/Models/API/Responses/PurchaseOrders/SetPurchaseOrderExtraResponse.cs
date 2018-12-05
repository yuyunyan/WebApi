using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    [DataContract]
    public class SetPurchaseOrderExtraResponse
    {
        [DataMember(Name = "poExtraId")]
        public int POExtraId { get; set; }

        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }
    }
}
