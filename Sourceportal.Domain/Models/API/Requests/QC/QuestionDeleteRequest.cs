using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.QC
{
    [DataContract]
   public class QuestionDeleteRequest
    {
        [DataMember(Name = "questionId")]
        public int QuestionId { get; set; }
    }
}
