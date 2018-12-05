using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.Security;
using Sourceportal.Domain.Models.API.Responses.Roles;
using Sourceportal.Domain.Models.API.Responses.Security;

namespace SourcePortal.Services.Roles
{
   public interface IRoleService
    {
        Response<RoleDetailsResponse> GetRoleDetails(int roleId);
        RoleCreateResponse InsertUpdateDeleteRole(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest);
        Response<RoleDetailsResponse> GetDataToCreateNewRole(ObjectType objectType);
        UserRolesResponse GetRolesForUser(int userId);
        UserRoleSetResponse SaveUserRoles(UserRolesSaveRequest userRolesSaveRequest);
        ObjectTypeSecuritiesGetResponse GetObjectTypeSecurityList();
        UserRolesResponse GetAllRoles();
        SecurityTypeListGetResponse GetTypeOptions();
        FilterObjectListGetResponse GetFilterObjects();
        UserNavigationRolesGetResponse GetUserNavigationRoles(int userId);
        UserNavigationRoleSetResponse SetUserNavigationRole(
            UserNavigationRoleSetRequest navigationRoleSetRequest);

        RoleTypeOptionsGetResponse GetRoleTypeOptions();
    }
}