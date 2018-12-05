using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.VendorRfqs
{
    [DataContract]
    public class VendorRfqLinesGetRequest
    {
        [DataMember(Name = "rfqId")]
        public int RfqId { get; set; }

        [DataMember(Name = "rfqLineId")]
        public int RfqLineId { get; set; }

        [DataMember(Name = "rowOffset")]
        public int RowOffset { get; set; }

        [DataMember(Name = "rowLimit")]
        public int RowLimit { get; set; }

        [DataMember(Name = "sortBy")]
        public string SortBy { get; set; }

        [DataMember(Name = "descSort")]
        public int DescSort { get; set; }

        [DataMember(Name = "partNumberStrip")]
        public string PartNumberStrip { get; set; }

        [DataMember(Name = "statusId")]
        public int? StatusId { get; set; }
    }
}
