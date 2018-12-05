using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember(Name = "isSuccess")]
        public bool IsSuccess { get; set; }
        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
