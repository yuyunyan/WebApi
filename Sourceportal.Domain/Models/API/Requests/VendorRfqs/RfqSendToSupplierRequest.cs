using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.VendorRfqs
{
    [DataContract]
    public class RfqSendToSupplierRequest
    {
        [DataMember(Name = "suppliers")]
        public List<VendorRfqSaveRequest> Suppliers;
        [DataMember(Name = "lines")]
        public List<RfqLineSaveRequest> Lines;
        [DataMember(Name = "comment")]
        public string Comment { get; set; }
    }
}
