using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    public class SetQuoteDetailsResponse : BaseResponse
    {
        [DataMember(Name = "QuoteId")]
        public int QuoteId { get; set; }
        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }
    }
}
