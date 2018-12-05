using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.ErrorLog
{
    [DataContract]
    public class ErrorLogDetailResponse : ErrorLogResponse
    {
        [DataMember(Name = "stackTrace")]
        public string StackTrace { get; set; }
        [DataMember(Name = "innerExceptionMessage")]
        public string InnerExceptionMessage { get; set; }
        [DataMember(Name = "postData")]
        public string PostData { get; set; }
    }
}
