using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Roles
{
    [DataContract]
    public class RoleTypeOptionsGetResponse : BaseResponse
    {
        [DataMember(Name = "roleTypeOptions")]
        public List<RoleTypeOption> RoleTypeOptions { get; set; }
    }

    [DataContract]
    public class RoleTypeOption
    {
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "objectName")]
        public string ObjectName { get; set; }
    }
}
