using Sourceportal.Domain.Models.API.Requests.ItemStock;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.QC;
using Sourceportal.Domain.Models.API.Responses.Sync;

namespace SourcePortal.Services.QC
{
    public interface IInspectionService
    {
        InspectionDetailsResponse GetInspectionDetails(int inspectionID);
        ItemStockWithBreakdownsListResponse GetStockListForInspection(int inspectionId);
        int SetItemStockOnInspection(SetItemStockRequest itemStock);
        int DeleteItemStockOnInspection(int stockId);
        int SetItemStockBreakdownOnInspection(SetItemStockBreakdownRequest breakdown);
        InspectionCheckListsResponse GetInspectionCheckLists(int inspectionId);
        InspectionConclusionResponse GetInspectionConclusion(int inspectionId);
        int? SetInspectionConclusion(InpsectionConclusionRequest request);
        BaseResponse SaveAnswer(SaveAnswerRequest saveAnswerRequest);
        InspectionGridResponse GetInspections(string searchString,int rowOffset, int rowLimit, string sortCol, bool descSort);
        BaseResponse SetInspection(SetInspectionFromSapRequest request);
        SyncResponse Sync(int inspectionId);
        SyncResponse SyncInspectionConclusion(int inspectionId);
        InspectionCheckListsResponse GetAvailableCheckLists(int inspectionId);
        QCanswersResponse SaveChecklistForInpection(InsertChecklistForInspectionRequest questionAnswersInsertRequest);
        BaseResponse DeleteChecklistForInpection(InsertChecklistForInspectionRequest questionAnswersInsertRequest);
        QCResultsResponse GetQCResults();
        BaseResponse UpdateInspectionResult(InpsectionConclusionRequest inpsectionResultRequest);
    }
}
