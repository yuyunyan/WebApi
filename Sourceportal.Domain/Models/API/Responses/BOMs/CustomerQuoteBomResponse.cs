using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class CustomerQuoteBomResponse : BaseResponse
    {
        [DataMember(Name = "quoteLines")]
        public List<RFQLineBom> QuoteLines { get; set; }

        [DataMember(Name = "TotalRows")]
        public int TotalRows { get; set; }
    }
}
