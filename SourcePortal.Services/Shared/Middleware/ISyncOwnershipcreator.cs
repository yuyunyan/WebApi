using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.Middleware.Owners;

namespace SourcePortal.Services.Shared.Middleware
{
    public interface ISyncOwnershipCreator
    {
        SyncOwnership Create(int id, ObjectType objectType);
    }
}