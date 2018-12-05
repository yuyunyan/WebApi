using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
   public class QCanswersResponse
    {
        [DataMember(Name = "addedByUser")]
        public bool AddedByUser { get; set; }
        [DataMember(Name = "isSuccess")]
        public bool IsSuccess { get; set; }
        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
