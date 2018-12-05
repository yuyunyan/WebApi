using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Security
{
    [DataContract]
    public class SecurityTypeListGetResponse
    {
        [DataMember(Name = "types")]
        public List<SecurityType> Types { get; set; }
    }

    [DataContract]
    public class SecurityType
    {
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "objectName")]
        public string ObjectName { get; set; }
    }
}
