using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses
{
    [DataContract]
    public class RoleCreateResponse
    {
        [DataMember(Name = "roleId")]
        public int RoleID;
    }
}
