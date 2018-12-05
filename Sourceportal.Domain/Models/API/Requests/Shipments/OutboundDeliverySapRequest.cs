using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Shipments
{
    [DataContract]
    public class OutboundDeliverySapRequest
    {
        [DataMember(Name = "externalId")]
        public string ExternalId { get; private set; }

        [DataMember(Name = "lines")]
        public List<OutboundDeliveryLine> Lines { get; set; }

        [DataMember(Name = "date")]
        public string Date { get; set; }

        [DataMember(Name = "trackingNumber")]
        public string TrackingNumber { get; set; }

        [DataMember(Name = "uuid")]
        public string UUID { get; set; }
    }

    [DataContract]
    public class OutboundDeliveryLine
    {
        [DataMember(Name = "salesOrderExternalId")]
        public string SalesOrderExternalId { get; set; }

        [DataMember(Name = "lineNumber")]
        public string LineNumber { get; set; }

        [DataMember(Name = "qty")]
        public decimal Qty { get; set; }

        [DataMember(Name = "partNumberExternalId")]
        public string PartNumberExternalId { get; set; }
    }
}
