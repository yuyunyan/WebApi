using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.QcInspection;

namespace SourcePortal.Services.QC
{
    public interface IQcInspectionSyncRequestCreator
    {
        MiddlewareSyncRequest<QcInspectionSync> Create(int inspectionId);
    }
}