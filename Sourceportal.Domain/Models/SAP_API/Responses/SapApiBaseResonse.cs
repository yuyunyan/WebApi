using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.SAP_API.Responses
{
    [DataContract]
    public class SapApiBaseResonse
    {
        [DataMember(Name = "severityCode")]
        public int SeverityCode { get; set; }

        [DataMember(Name = "severityCodeMessage")]
        public string SeverityCodeMessage { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
