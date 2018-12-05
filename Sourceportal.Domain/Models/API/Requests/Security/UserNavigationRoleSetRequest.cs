using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Security
{
    [DataContract]
    public class UserNavigationRoleSetRequest
    {
        [DataMember(Name = "roleId")]
        public int RoleID { get; set; }
        [DataMember(Name = "userId")]
        public int UserID { get; set; }
        [DataMember(Name = "userRoleId")]
        public int? UserRoleID { get; set; }
        [DataMember(Name = "isDeleted")]
        public int IsDeleted { get; set; }
    }
}
