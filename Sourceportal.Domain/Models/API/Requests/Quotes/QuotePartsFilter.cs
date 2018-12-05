using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Quotes
{
    [DataContract]
    public class QuotePartsFilter
    {
        [DataMember(Name = "quoteId")]
        public int QuoteID { get; set; }
        [DataMember(Name = "versionId")]
        public int VersionID { get; set; }
        [DataMember(Name = "filterBy")]
        public string FilterBy { get; set; }
        [DataMember(Name = "filterText")]
        public string FilterText { get; set; }
    }
}
