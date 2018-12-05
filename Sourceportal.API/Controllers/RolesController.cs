using System.Web.Http;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses;
using SourcePortal.Services.Roles;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.Security;
using Sourceportal.Domain.Models.API.Responses.Roles;
using Sourceportal.Domain.Models.API.Responses.Security;

namespace Sourceportal.API.Controllers
{
    public class RolesController : ApiController
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Route("api/roles/getdetails")]
        [Authorize]
        [HttpGet]
        public Response<RoleDetailsResponse> GetRoleDetails(int roleId)
        {
            return _roleService.GetRoleDetails(roleId);
        }

        [Route("api/roles/insertUpdateDeleteRole")]
        [Authorize]
        [HttpPost]
        public RoleCreateResponse InsertUpdateDeleteRole(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest)
        {
            return _roleService.InsertUpdateDeleteRole(roleCreateDeleteRequest);
        }

        [Route("api/roles/getDetailsToCreate")]
        [Authorize]
        [HttpGet]
        public Response<RoleDetailsResponse> GetDetailsToCreate(int objectTypeId)
        {
            return _roleService.GetDataToCreateNewRole((ObjectType)objectTypeId);
        }

        [Route("api/roles/getuserroles")]
        [Authorize]
        [HttpGet]
        public UserRolesResponse GetUserRoles(int userId)
        {
            return _roleService.GetRolesForUser(userId);
        }

        [Route("api/roles/saveUserRole")]
        [Authorize]
        [HttpPost]
        public UserRoleSetResponse SaveUserRole(UserRolesSaveRequest userRolesSaveRequest)
        {
            return _roleService.SaveUserRoles(userRolesSaveRequest);
        }

        [Authorize]
        [Route("api/roles/getObjectTypeSecurities")]
        [HttpGet]
        public ObjectTypeSecuritiesGetResponse GetObjectTypeSecurities()
        {
            return _roleService.GetObjectTypeSecurityList();
        }

        [Authorize]
        [Route("api/roles/getAllRoles")]
        [HttpGet]
        public UserRolesResponse GetAllRoles()
        {
            return _roleService.GetAllRoles();
        }

        [Authorize]
        [Route("api/roles/getTypeOptions")]
        [HttpGet]
        public SecurityTypeListGetResponse GetTypeOptions()
        {
            return _roleService.GetTypeOptions();
        }

        [Authorize]
        [Route("api/roles/getFilterObjectList")]
        [HttpGet]
        public FilterObjectListGetResponse GetFilterObjectList()
        {
            return _roleService.GetFilterObjects();
        }

        [Authorize]
        [Route("api/roles/getNavigationRoles")]
        [HttpGet]
        public UserNavigationRolesGetResponse GetUserNavigationRoles(int userId)
        {
            return _roleService.GetUserNavigationRoles(userId);
        }

        [Authorize]
        [Route("api/roles/setNavigationRole")]
        [HttpPost]
        public UserNavigationRoleSetResponse SetUserNavigationRole(
            UserNavigationRoleSetRequest navigationRoleSetRequest)
        {
            return _roleService.SetUserNavigationRole(navigationRoleSetRequest);
        }

        [Authorize]
        [Route("api/roles/getRoleTypeOptions")]
        [HttpGet]
        public RoleTypeOptionsGetResponse GetRoleTypeOptions()
        {
            return _roleService.GetRoleTypeOptions();
        }
    }
}
