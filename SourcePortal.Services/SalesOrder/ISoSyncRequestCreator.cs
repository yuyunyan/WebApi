using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.SalesOrder;
using Sourceportal.Domain.Models.SAP_API.Requests;

namespace SourcePortal.Services.SalesOrder
{
    public interface ISoSyncRequestCreator
    {
        MiddlewareSyncRequest<SalesOrderSync> Create(int soId, int versionId);
    }
    
}