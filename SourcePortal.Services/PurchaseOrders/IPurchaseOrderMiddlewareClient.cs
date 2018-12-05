

namespace SourcePortal.Services.PurchaseOrders
{
    using Sourceportal.Domain.Models.API.Responses.Sync;

    public interface IPurchaseOrderMiddlewareClient
    {
        SyncResponse Sync(int soId, int versionId);
    }
}
