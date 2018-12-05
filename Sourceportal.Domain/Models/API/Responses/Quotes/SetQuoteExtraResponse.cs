using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
   [DataContract]
   public class SetQuoteExtraResponse
    {
        [DataMember(Name = "quoteExtraId")]
        public int QuoteExtraId { get; set; }
    }
}
