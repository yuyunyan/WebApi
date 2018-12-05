using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    [DataContract]
   public class PurchaseOrderDetailsSetResponse : BaseResponse
    {
        [DataMember(Name = "purchaseOrderId")]
        public int PurchaseOrderId { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }
    }
}
