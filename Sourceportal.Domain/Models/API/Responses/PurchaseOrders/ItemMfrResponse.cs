using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    [DataContract]
    public class ItemMfrResponse : BaseResponse
    {
        [DataMember(Name = "mfr")]
        public string MfrName { get; set; }
    }
}
