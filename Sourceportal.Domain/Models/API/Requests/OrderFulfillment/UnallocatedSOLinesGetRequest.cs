using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.OrderFulfillment
{
    [DataContract]
    public class UnallocatedSOLinesGetRequest
    {
        [DataMember(Name = "poLineId")]
        public int PoLineId { get; set; }
        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }
        [DataMember(Name = "soLineId")]
        public int? SoLineId { get; set; }
        [DataMember(Name = "includeUnallocated")]
        public bool IncludeUnallocated { get; set; }
    }
}
