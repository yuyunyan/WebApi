using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses
{
    [DataContract]
    public class UserRolesResponse
    {
        [DataMember(Name = "roles")]
        public List<UserRole> UserRoles;
    }

    [DataContract]
    public class UserRole
    {
        [DataMember(Name = "userRoleId")]
        public int UserRoleID { get; set; }
        [DataMember(Name = "roleId")]
        public int RoleId;
        [DataMember(Name = "roleName")]
        public string RoleName;
        [DataMember(Name = "objectTypeName")]
        public string ObjectTypeName;
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeId;
        [DataMember(Name = "selectedForUser")]
        public bool SelectedForUser;
        [DataMember(Name = "filter")]
        public string Filter { get; set; }
        [DataMember(Name = "filterTypeId")]
        public int FilterTypeID { get; set; }
        [DataMember(Name = "filterObject")]
        public string FilterObject { get; set; }
        [DataMember(Name = "filterObjectId")]
        public int FilterObjectID { get; set; }
        [DataMember(Name = "filterObjectTypeId")]
        public int FilterObjectTypeID { get; set; }
        [DataMember(Name = "typeDescription")]
        public string TypeDescription { get; set; }
        [DataMember(Name = "typeSecurityId")]
        public int TypeSecurityID { get; set; }
    }
}
