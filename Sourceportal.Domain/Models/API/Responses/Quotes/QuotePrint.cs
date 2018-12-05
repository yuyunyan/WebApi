using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
    public class QuotePrint : BaseResponse
    {
        [DataMember(Name = "quoteLineId")]
        public int QuoteLineId { get; set; }

        [DataMember(Name = "isPrinted")]
        public bool IsPrinted { get; set; }
    }
}
