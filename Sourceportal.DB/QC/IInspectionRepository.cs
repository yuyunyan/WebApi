using Sourceportal.Domain.Models.DB.QC;
using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.DB.shared;
using Sourceportal.Domain.Models.API.Responses.QC;
using Sourceportal.Domain.Models.DB.ItemStock;
using Sourceportal.Domain.Models.API.Requests.ItemStock;

namespace Sourceportal.DB.QC
{
    public interface IInspectionRepository
    {
        InspectionDb GetInspectionDetails(int inspectionId);
        List<ItemStockBreakdownDb> GetItemStockBreakdownList(int stockId);
        int SetItemStockBreakdown(SetItemStockBreakdownRequest isb);
        bool DeleteItemStockBreakdownLines(int StockID);
        List<int> GetStockIdsOnInpsections(int inspectionId);
        IList<ChecklistDb> GetCheckListsForInspectionWithQuestions(int inspectionId);
        ConclusionDb GetInspectionConclusion(int inspectionId);
        bool SaveAnswer(SaveAnswerRequest saveAnswerRequest);
        IList<InspectionGridItemDb> GetInspectionList(string searchString,int rowOffset, int rowLimit, string sortCol, bool descSort);
        int? SetInspectionConclusion(InpsectionConclusionRequest request);
        BaseDbResult CreateInspection(SetInspectionFromSapRequest request, int itemInventoryId);
        string GetInspectionStatusExternalId(int inspectionStatusId);
        bool InspectionExistsByExternalId(string requestExternalId);
        BaseDbResult UpdateInspection(SetInspectionFromSapRequest request);
        IList<ChecklistDb> getAllNewCheckLists(int inspectionId);

        IList<ChecklistDb> GetCheckListsWithQuestionsAndAnswers(int? inspectionId);
        IList<ChecklistDb> GetCheckLists();
        IList<ChecklistQuestionDb> GetQuestionForCheckList(int checkListId);
        QCAnswerDb SaveChecklistForInpection(InsertQuestionsToAnswersRequest questionAnswersInsertRequest);
        int DeleteChecklistForInpection(InsertQuestionsToAnswersRequest questionAnswersInsertRequest);
        IList<QCResultsDb> GetQCResults();
        int UpdateInspectionResult(InpsectionConclusionRequest inpsectionResultRequest);
        void UpdateInspectionCompletedFields(int inspectionId);
    }
}
