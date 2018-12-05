using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class ContactJobFunctionListGetReponse
    {
        [DataMember(Name = "contactJobFunctions")]
        public List<ContactJobFunction> ContactJobFunctions { get; set; }
    }

    [DataContract]
    public class ContactJobFunction
    {
        [DataMember(Name = "jobFunctionId")]
        public int JobFunctionID { get; set; }
        [DataMember(Name = "jobFunctionName")]
        public string JobFunctionName { get; set; }
    }
}
