using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.Security;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB.Roles;

namespace Sourceportal.DB.Roles
{
    public interface IRoleRepository
    {
        List<DbNavigationLink> GetNavigationLinksForType();
        List<DbNavigationLink> GetNavigationLinksForRole(int roleId);
        List<DbField> GetFieldsForType(ObjectType objectType);
        List<DbField> GetFieldsForRole(int roleId);
        List<DbPermission> GetPermissionsForType(ObjectType objectType);
        List<DbPermission> GetPermissionsForRole(int roleId);
        DbRoleCreated InsertUpdateDeleteRole(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest, int userId);
        RoleDetailsResponse GetRoleStaticData(int roleId);
        List<DbRole> GetRolesForUser(int userId);
        List<DbRole> GetAllRoles();
        DbRole SaveUserRole(UserRolesSaveRequest userRolesSaveRequest);
        List<DbObjectTypeSecurity> GetObjectTypeSecurityList();
        List<DbSecurityType> GetTypeOptions();
        List<DbFilterObject> GetFilterObjects();
        List<DbNavRole> GetUserNavigationRoles(int userId);
        DbNavRole SetUserNavigationRole(UserNavigationRoleSetRequest navigationRoleSetRequest);
        List<DbRoleTypeOption> GetRoleTypeOptions();
        int InsertUpdateDeletePermissions(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest, int userId, int roleId);
        int InsertUpdateDeleteFields(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest, int userId, int roleId);
        int InsertUpdateDeleteNavLinks(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest, int userId, int roleId);
    }
}
