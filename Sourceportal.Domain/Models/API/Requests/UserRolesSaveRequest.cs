using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests
{
    [DataContract]
    public class UserRolesSaveRequest
    {
        [DataMember(Name = "userRoleId")]
        public int UserRoleID { get; set; }
        [DataMember(Name = "userId")]
        public int UserID { get; set; }
        [DataMember(Name = "roleId")]
        public int RoleID { get; set; }
        [DataMember(Name = "typeSecurityId")]
        public int TypeSecurityID { get; set; }
        [DataMember(Name = "filterObjectId")]
        public int FilterObjectID { get; set; }
        [DataMember(Name = "isDeleted")]
        public int IsDeleted { get; set; }
    }
}
