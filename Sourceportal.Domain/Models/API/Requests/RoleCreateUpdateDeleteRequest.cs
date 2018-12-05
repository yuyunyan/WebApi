using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests
{
    [DataContract]
    public class RoleCreateUpdateDeleteRequest
    {
        [DataMember(Name = "roleId")]
        public int RoleId { get; set; }
        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }
        [DataMember(Name = "roleName")]
        public string RoleName { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeId { get; set; }
        [DataMember(Name = "permissionData")]
        public List<PermissionCreateData> PermissionData { get; set; }
        [DataMember(Name = "fieldData")]
        public List<FieldCreateData> FieldData { get; set; }
        [DataMember(Name = "navLinkData")]
        public List<NavLinkCreateData> NavLinkData { get; set; }
    }

    public class PermissionCreateData
    {
        public int PermissionID;
    }

    public class FieldCreateData
    {
        public int FieldID;
        public int CanEdit;
    }

    public class NavLinkCreateData
    {
        public int NavID;
    }
}
