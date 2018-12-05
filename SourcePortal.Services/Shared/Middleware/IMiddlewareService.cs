using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.SalesOrder;

namespace SourcePortal.Services.Shared.Middleware
{
    public interface IMiddlewareService
    {
        SyncResponse Sync<T>(MiddlewareSyncRequest<T> rqeuest) where T : MiddlewareSyncBase;       

        TResponse SynchronousSync<T, TResponse>(MiddlewareSyncRequest<T> rqeuest, string path) where T : MiddlewareSyncBase where TResponse : BaseResponse;

        bool IsTherePendingTransactions(string objectType,int objectId, string externalId);

        SyncResponse Sync<T>(T rqeuest, string path) where T : MiddlewareSyncRequest;
    }
}