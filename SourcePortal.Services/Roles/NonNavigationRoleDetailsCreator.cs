using System.Collections.Generic;
using System.Linq;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Roles;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB.Roles;

namespace SourcePortal.Services.Roles
{
    public class NonNavigationRoleDetailsCreator : IRoleDetailsCreator
    {
        public Response<RoleDetailsResponse> Create(ObjectType objectType, int? roleId, IRoleRepository roleRepository)
        {
            var roleDetails = new Response<RoleDetailsResponse>();
            roleDetails.Data = new RoleDetailsResponse();

            roleDetails.Data.Fields =  SetFileds(objectType, roleId, roleRepository, roleDetails);
            roleDetails.Data.Permissions = SetPermissions(objectType, roleId, roleRepository);
            
            return roleDetails;
        }

        private static List<Permission> SetPermissions(ObjectType objectType, int? roleId, IRoleRepository roleRepository)
        {
            var fullDbPermssionList = roleRepository.GetPermissionsForType(objectType);
            var permissionsForRole = roleId != null ? roleRepository.GetPermissionsForRole(roleId.Value) : new List<DbPermission>();
            var permissionIds = permissionsForRole.Where(x => x.IsDeleted == 0).Select(x => x.PermissionID).ToList();

            var permissionList = new List<Permission>();
            foreach (var dbPermission in fullDbPermssionList)
            {
                var permissionRes = new Permission
                {
                    PermissionID = dbPermission.PermissionID,
                    PermName = dbPermission.PermName,
                    SelectedForRole = permissionIds.Contains(dbPermission.PermissionID),
                    Description = dbPermission.Description
                };
                if (roleId != null)
                {
                    permissionRes.RoleID = (int) roleId;
                }
                permissionList.Add(permissionRes);
            }
            return permissionList;
        }

        private static List<Field> SetFileds(ObjectType objectType, int? roleId, IRoleRepository roleRepository, Response<RoleDetailsResponse> roleDetails)
        {
            var allFields = roleRepository.GetFieldsForType(objectType);
            var fieldsForRole = roleId != null ?roleRepository.GetFieldsForRole(roleId.Value) : new List<DbField>();
            var roleFieldIds = fieldsForRole.Select(x => x.FieldId).ToList();

            var fieldList = new List<Field>();
            foreach (var dbField in allFields)
            {
                var fieldData = fieldsForRole.Find(x => x.FieldId == dbField.FieldId);
                var resField = new Field
                {
                    FieldName = dbField.FieldName,
                    FieldID = dbField.FieldId,
                    SelectedForRole = roleFieldIds.Contains(dbField.FieldId) && fieldData.IsDeleted == 0,
                    IsEditable = roleFieldIds.Contains(dbField.FieldId) &&
                                 fieldsForRole.Any(x => x.FieldId == dbField.FieldId && x.CanEdit == 1 && x.IsDeleted == 0),
                    FieldType = dbField.FieldType
                };

                if (roleId != null)
                {
                    resField.RoleID = (int) roleId;
                }
                if (fieldData != null)
                {
                    resField.IsDeleted = fieldData.IsDeleted;
                }
                fieldList.Add(resField);
            }

            return fieldList;
        }
    }
}
