using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses
{
    [DataContract]
    public class ObjectTypeIdResponse
    {
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeId { get; set; }
    }
}
