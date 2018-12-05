using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Security
{
    [DataContract]
    public class UserNavigationRolesGetResponse : BaseResponse
    {
        [DataMember(Name = "navigationRoles")]
        public List<UserNavigationRole> NavigationRoles { get; set; }
    }

    [DataContract]
    public class UserNavigationRole
    {
        [DataMember(Name = "roleId")]
        public int RoleID { get; set; }
        [DataMember(Name = "roleName")]
        public string RoleName { get; set; }
        [DataMember(Name = "userRoleId")]
        public int? UserRoleID { get; set; }
        [DataMember(Name = "isDeleted")]
        public int? IsDeleted { get; set; }
    }
}
