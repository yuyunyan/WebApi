using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.ErrorLog
{
    [DataContract]
    public class ErrorLogListResponse : BaseResponse
    {
        [DataMember(Name = "errorLogList")]
        public List<ErrorLogResponse> ErrorLogList { get; set; }
        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount { get; set; }
    }

    [DataContract]
    public class ErrorLogResponse
    {
        [DataMember(Name = "errorId")]
        public int ErrorID { get; set; }
        [DataMember(Name = "url")]
        public string URL { get; set; }
        [DataMember(Name = "exceptionType")]
        public string ExceptionType { get; set; }
        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }
        [DataMember(Name = "application")]
        public string Application { get; set; }
        [DataMember(Name = "user")]
        public string User { get; set; }
        [DataMember(Name = "timestamp")]
        public string Timestamp { get; set; }
    }
}
