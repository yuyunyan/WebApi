using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
    public class QuoteToSOResponse : BaseResponse
    {
        [DataMember(Name = "soId")]
        public int SalesOrderId { get; set; }

        [DataMember(Name = "soVersionId")]
        public int VersionId { get; set; }

        [DataMember(Name = "numOfLinesCopied")]
        public int LinesCopiedCount { get; set; }

        [DataMember(Name = "numOfExtrasCopied")]
        public int ExtrasCopiedCount { get; set; }
    }
}
