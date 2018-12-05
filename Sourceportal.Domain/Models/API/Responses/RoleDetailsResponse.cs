using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses
{
    [DataContract]
    public class RoleDetailsResponse
    {
        [DataMember(Name = "permissions")]
        public List<Permission> Permissions;
        [DataMember(Name = "fields")]
        public List<Field> Fields;
        [DataMember(Name = "navigationLinks")]
        public List<NavigationLink> NavigationLinks;
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeId;
        [DataMember(Name = "roleName")]
        public string RoleName;
        [DataMember(Name = "roleId")]
        public int RoleID { get; set; }
    }

    [DataContract]
    public class NavigationLink
    {
        [DataMember(Name = "navId")]
        public int NavId { get; set; }
        [DataMember(Name = "navName")]
        public string NavName { get; set; }
        [DataMember(Name = "isLink")]
        public bool IsLink { get; set; }
        [DataMember(Name = "childNodes")]
        public List<NavigationLink> ChildNodes { get; set; }
        [DataMember(Name = "roleId")]
        public int RoleId { get; set; }
        [DataMember(Name = "selectedForRole")]
        public bool SelectedForRole { get; set; }
    }

    [DataContract]
    public class Permission
    {
        [DataMember(Name = "permissionId")]
        public int PermissionID { get; set; }
        [DataMember(Name = "permName")]
        public string PermName { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "selectedForRole")]
        public bool SelectedForRole { get; set; }
        [DataMember(Name = "roleId")]
        public int RoleID { get; set; }
    }

    [DataContract]
    public class Field
    {
        [DataMember(Name = "fieldId")]
        public int FieldID { get; set; }
        [DataMember(Name = "fieldName")]
        public string FieldName { get; set; }
        [DataMember(Name = "selectedForRole")]
        public bool SelectedForRole { get; set; }
        [DataMember(Name = "isEditable")]
        public bool IsEditable { get; set; }
        [DataMember(Name = "roleId")]
        public int RoleID { get; set; }
        [DataMember(Name = "isDeleted")]
        public int IsDeleted { get; set; }
        [DataMember(Name = "fieldType")]
        public string FieldType { get; set; }
    }
}
