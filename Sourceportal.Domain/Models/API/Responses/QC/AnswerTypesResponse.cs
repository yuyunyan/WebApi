using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class AnswerTypesResponse
    {
        [DataMember(Name = "answerTypes")]
        public List<TypeResponse> AnswerTypes { get; set; }
    }

    [DataContract]
    public class TypeResponse
    {
        [DataMember(Name = "answerTypeId")]
        public int AnswerTypeID { get; set; }

        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }
    }
}
