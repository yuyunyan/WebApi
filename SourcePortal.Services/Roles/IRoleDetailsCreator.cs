using Sourceportal.DB.Enum;
using Sourceportal.DB.Roles;
using Sourceportal.Domain.Models.API.Responses;

namespace SourcePortal.Services.Roles
{
    public interface IRoleDetailsCreator
    {
        Response<RoleDetailsResponse> Create(ObjectType objectType, int? roleId, IRoleRepository roleRepository);
    }
}