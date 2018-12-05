using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.ErrorLog
{
    [DataContract]
    public class LogToDbRequest
    {
        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "exceptionType")]
        public string ExceptionType { get; set; }

        [DataMember(Name = "postData")]
        public string PostData { get; set; }

        [DataMember(Name = "stackTrace")]
        public string StackTrace { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "application")]
        public string Application { get; set; }

        [DataMember(Name = "innerException")]
        public string InnerException { get; set; }
    }
}
