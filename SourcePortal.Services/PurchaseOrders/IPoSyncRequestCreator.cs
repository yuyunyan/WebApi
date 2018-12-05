using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.PurchaseOrder;

namespace SourcePortal.Services.PurchaseOrders
{
    public interface IPoSyncRequestCreator
    {
        MiddlewareSyncRequest<PurchaseOrderSync> Create(int soId, int versionId, int? soLineId = null, int? poLineId = null);       
    }
}