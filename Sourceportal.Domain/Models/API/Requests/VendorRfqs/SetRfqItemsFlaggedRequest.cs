using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.VendorRfqs
{
    [DataContract]
    public class SetRfqItemsFlaggedRequest
    {
        [DataMember(Name = "rfqDetails")]
        public VendorRfqSaveRequest RfqDetails { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "rfqLines")]
        public List<RfqLineSaveRequest> RfqLines { get; set; }
    }
}
