using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses
{
    [DataContract]
    public class UserRoleResponse
    {
        [DataMember(Name = "roleId")]
        public int RoleID { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "roleName")]
        public string RoleName { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}