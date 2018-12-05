using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses
{
    [DataContract]
    public class Response<T> where T : class 
    {
        [DataMember(Name = "isSuccess")]
        public bool IsSuccess { get; set; }
        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }
        [DataMember(Name = "data")]
        public T Data { get; set; }
    }
}
