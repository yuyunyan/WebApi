using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.OrderFulfillment
{
    [DataContract]
    public class LogisticsExecutionSapRequest
    {
        [DataMember(Name = "oldInventory")]
        public InboundDeliveryItem OldInventory { get; set; }

        [DataMember(Name = "changedInventory")]
        public InboundDeliveryItem ChangedInventory { get; set; }
    }
}
