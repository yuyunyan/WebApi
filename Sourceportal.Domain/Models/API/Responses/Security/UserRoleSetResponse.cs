using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Security
{
    [DataContract]
    public class UserRoleSetResponse : BaseResponse
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
