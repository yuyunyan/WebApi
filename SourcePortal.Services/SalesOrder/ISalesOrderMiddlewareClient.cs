namespace SourcePortal.Services.SalesOrder
{
    using Sourceportal.Domain.Models.API.Responses.Sync;
    using Sourceportal.Domain.Models.Middleware;
    using Sourceportal.Domain.Models.Middleware.SalesOrder;

    public interface ISalesOrderMiddlewareClient
    {
        SyncResponse Sync(int soId, int versionId);
    }
}
