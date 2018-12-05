using Dapper;
using Sourceportal.Domain.Models.DB.QC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.QC;
using Sourceportal.Domain.Models.DB.shared;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.DB.ItemStock;
using Sourceportal.Domain.Models.API.Requests.ItemStock;

namespace Sourceportal.DB.QC
{
    public class InspectionRepository : IInspectionRepository
    {

        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public InspectionDb GetInspectionDetails(int inspectionId)
        {
            InspectionDb inspectionDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@InspectionID", inspectionId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                param.Add("@UserID", UserHelper.GetUserId());

                var res = con.Query<InspectionDb>("uspQCInspectionGet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = $"Database error occured: Inspection id:{inspectionId} not found.";
                    throw new GlobalApiException(errorMessage);
                }
                 
                inspectionDb = res.First();
                con.Close();
            }

            return inspectionDb;
        }

        public List<ItemStockBreakdownDb> GetItemStockBreakdownList(int stockId)
        {
            var isbDb = new List<ItemStockBreakdownDb>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@StockID", stockId);
                isbDb = con.Query<ItemStockBreakdownDb>("uspItemStockBreakdownGet", param, commandType: CommandType.StoredProcedure).ToList();

                con.Close();
            }
            return isbDb;
        }

        public int SetItemStockBreakdown(SetItemStockBreakdownRequest isb)
        {
            int breakdown;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                
                param.Add("@BreakdownID", isb.StockBreakdownID);
                param.Add("@StockID", isb.StockID);
                param.Add("@DateCode", isb.DateCode);
                param.Add("@PackagingID", isb.PackagingTypeID);
                param.Add("@PackageConditionID", isb.PackageConditionID);
                param.Add("@COO", isb.COO);
                param.Add("@Expiry", isb.Expiry);
                param.Add("@PackQty", isb.PackQty);
                param.Add("@NumPacks", isb.NumPacks);
                param.Add("@IsDiscrepant", isb.IsDiscrepant);
                param.Add("@IsDeleted", isb.IsDeleted);
                param.Add("@MfrLotNum", isb.MfrLotNum);
                param.Add("@UserID", UserHelper.GetUserId());

                breakdown = con.Query<int>("uspItemStockBreakdownSet", param,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                con.Close();
            }

            return breakdown;
        }

        public bool DeleteItemStockBreakdownLines(int StockID)
        {
            int rowCount;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@StockID", StockID);
                param.Add("@UserID", UserHelper.GetUserId());
                rowCount = con.Query<int>("UPDATE ItemStockBreakdown SET IsDeleted = 1, Modified=GETUTCDATE(), ModifiedBy=@userID WHERE StockID = @StockID SELECT @@ROWCOUNT", param,
                    commandType: null).FirstOrDefault();

                con.Close();
            }

            return (rowCount > 0 ? true : false);
        }

        public List<int> GetStockIdsOnInpsections(int inspectionId)
        {
            var stockIdList = new List<int>();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@InspectionID", inspectionId);

                stockIdList = con.Query<int>("SELECT StockID FROM mapQCInspectionStock " +
                    "WHERE InspectionID = @InspectionID", param, commandType: null).ToList();

                con.Close();
            }

            return stockIdList;
        }

        public IList<ChecklistDb> GetCheckListsWithQuestionsAndAnswers(int? inspectionId)
        {
            IList<ChecklistDb> checkListsDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@InspectionID", inspectionId);
                
                checkListsDb = con.Query<ChecklistDb>("uspQCInspectionCheckListsGet", param, commandType: CommandType.StoredProcedure).ToList();
                
                con.Close();
            }

            return checkListsDb;
        }

        public IList<ChecklistDb> GetCheckLists()
        {
            IList<ChecklistDb> checkListsDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                checkListsDb = con.Query<ChecklistDb>("uspCheckListHasQuestion", commandType: CommandType.StoredProcedure).ToList();

                con.Close();
            }

            return checkListsDb;
        }

        public IList<ChecklistDb> GetCheckListsForInspectionWithQuestions(int inspectionId)
        {
            IList<ChecklistDb> checkListsDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@InspectionID", inspectionId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                checkListsDb = con.Query<ChecklistDb>("uspQCInspectionCheckListsGet", param, commandType: CommandType.StoredProcedure).ToList();

                foreach (var checklistDb in checkListsDb)
                {
                    IList<InspectionQuestionDb> questionsDb;
                    DynamicParameters checkListParams = new DynamicParameters();
                    checkListParams.Add("@ChecklistID", checklistDb.ChecklistId);
                    checkListParams.Add("@InspectionID", inspectionId);
                    questionsDb = con.Query<InspectionQuestionDb>("uspQCQuestionsGet", checkListParams, commandType: CommandType.StoredProcedure).ToList();
                    checklistDb.Questions = questionsDb;
                }

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = $"Database error occured: Checklist for Inspection id:{inspectionId} not found.";
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }

            return checkListsDb;
        }

        public IList<ChecklistQuestionDb> GetQuestionForCheckList(int checkListId)
        {
            IList<ChecklistQuestionDb> checkListsDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChecklistID", checkListId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                checkListsDb = con.Query<ChecklistQuestionDb>("uspQCCheckListQuestionGet", param, commandType: CommandType.StoredProcedure).ToList();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = $"Database error occured: Question for checklist id:{checkListId} not found.";
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }

            return checkListsDb;
        }

        public bool SaveAnswer(SaveAnswerRequest saveAnswerRequest)
        {
            var completedBy = 0;
            string completedDate = null;

            if (saveAnswerRequest.Inspected)
            {
                completedBy = UserHelper.GetUserId();
                completedDate = DateTime.Now.ToString();
            }
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AnswerID", saveAnswerRequest.AnswerId);
                param.Add("@Answer", saveAnswerRequest.Answer);
                param.Add("@QtyFailed", saveAnswerRequest.QtyFailed);
                param.Add("@Note", saveAnswerRequest.Comments);
                param.Add("@CompletedDate", completedDate);
                param.Add("@CompletedBy", completedBy);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                con.Query("uspInspectionAnswerSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", InspectionDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }

            return true;
        }

         public IList<InspectionGridItemDb> GetInspectionList(string searchString,int rowOffset, int rowLimit, string sortCol, bool descSort)
         {
            IList<InspectionGridItemDb> inspectionList;
            using (var con = new SqlConnection(ConnectionString))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SearchString", searchString);
                param.Add("@RowOffset",rowOffset);
                param.Add("@RowLimit", rowLimit);
                param.Add("@SortBy", sortCol);
                param.Add("@DescSort", descSort);
                param.Add("@UserID", UserHelper.GetUserId());

                con.Open();
                inspectionList = con.Query<InspectionGridItemDb>("uspQCInspectionListGet", param, commandType: CommandType.StoredProcedure).ToList();
                
                con.Close();
            }

            return inspectionList;
         }
        
        public ConclusionDb GetInspectionConclusion(int inspectionId)
        {
            ConclusionDb conclusionDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@InspectionID", inspectionId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<ConclusionDb>("uspQCConclusionGet", param,
                    commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = $"Database error occured: Inspection Colusion id:{inspectionId} not found.";
                    throw new GlobalApiException(errorMessage);
                }

                conclusionDb = res.First();
                con.Close();
            }

            return conclusionDb;
        }


        public int? SetInspectionConclusion(InpsectionConclusionRequest request)
        {
            int? ret = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@InspectionID", request.InspectionID);
                param.Add("@Conclusion", request.Conclusion);
                param.Add("@CreatedBy", UserHelper.GetUserId());
                ret = con.Query<int>("uspQCConclusionSet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();
            }

            return ret;
        }

        public BaseDbResult CreateInspection(SetInspectionFromSapRequest request, int stockId)
        {
            int statusId = InspectionStatusIdByExternal(request.InspectionStatusExternalId);
            return DbCommonFunctions.ExecuteStoreProcedure(CreateParamsForInspectionSet(request, stockId, statusId), "uspQCInspectionCreate", null);
        }

        public string GetInspectionStatusExternalId(int inspectionStatusId)
        {
            string externalId = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@statusID", inspectionStatusId);

                var result = con.Query<string>("SELECT ExternalID FROM lkpQCInspectionStatuses q WHERE InspectionStatusId=@statusID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    externalId = result.First();
                }

                con.Close();
            }

            return externalId;
        }

        public bool InspectionExistsByExternalId(string requestExternalId)
        {
            IEnumerable<dynamic> result;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", requestExternalId);

                result = con.Query("SELECT InspectionID FROM QCInspections WHERE ExternalID=@ExternalID", param, commandType: null);
                con.Close();
            }
            return result != null && result.Any();
        }

        public BaseDbResult UpdateInspection(SetInspectionFromSapRequest request)
        {
            int statusId = InspectionStatusIdByExternal(request.InspectionStatusExternalId);

            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", UserHelper.GetUserId());
            param.Add("@ExternalID", request.ExternalId);
            param.Add("@InspectionQty", request.InspectionQty);
            if (statusId > 0)
                param.Add("@InspectionStatusID", statusId);
            param.Add("@ret", direction: ParameterDirection.ReturnValue);
           
            return DbCommonFunctions.ExecuteStoreProcedure(param, "uspQCInspectionUpdate", null);
        }

        public int InspectionStatusIdByExternal(string externalId)
        {
            int statusId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@statusID", externalId);

                var result = con.Query<int>("SELECT InspectionStatusID FROM lkpQCInspectionStatuses q WHERE ExternalID=@statusID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    statusId = result.First();
                }

                con.Close();
            }

            return statusId;
        }

        public int InspectionTypeIdByExternal(string externalId)
        {
            int typeId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@typeId", externalId);

                var result = con.Query<int>("SELECT InspectionTypeID FROM lkpQcInspectionTypes q WHERE ExternalID=@typeId", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    typeId = result.First();
                }

                con.Close();
            }

            return typeId;
        }

        private static DynamicParameters CreateParamsForInspectionSet(SetInspectionFromSapRequest request, int stockId, int inspectionStatusId)
        {

            DynamicParameters param = new DynamicParameters();
            param.Add("@StockID", stockId);
            param.Add("@UserID", UserHelper.GetUserId());
            param.Add("@ExternalID", request.ExternalId);
            param.Add("@InspectionQty", request.InspectionQty);
            param.Add("@InspectionTypeExternalID", request.InspectionTypeExternalId);
            if (inspectionStatusId > 0)
                param.Add("@InspectionStatusID", inspectionStatusId);
            param.Add("@ret", direction: ParameterDirection.ReturnValue);
            return param;
        }

        public IList<ChecklistDb> getAllNewCheckLists(int inspectionId)
        {
            List<ChecklistDb> newCheckListsDb = new List<ChecklistDb>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                newCheckListsDb = con.Query<ChecklistDb>("uspQCInspectionCheckListsGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return newCheckListsDb;
        }

        public QCAnswerDb SaveChecklistForInpection(InsertQuestionsToAnswersRequest questionAnswersInsertRequest)
        {
            QCAnswerDb qCAnswerDb = new QCAnswerDb();
           using(var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@QuestionID", questionAnswersInsertRequest.QuestionID);
                param.Add("@QuestionVersionID", questionAnswersInsertRequest.QuestionVersionID);
                param.Add("@InspectionID", questionAnswersInsertRequest.InspectionID);
                param.Add("@CreatedBy", UserHelper.GetUserId());
                con.Open();
                qCAnswerDb = con.Query<QCAnswerDb>("uspAnswersSet",param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return qCAnswerDb;
        }

        public int DeleteChecklistForInpection(InsertQuestionsToAnswersRequest questionAnswersInsertRequest)
        {
            int statusId;
            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@QuestionID", questionAnswersInsertRequest.QuestionID);
                param.Add("@InspectionID", questionAnswersInsertRequest.InspectionID);
                con.Open();
                statusId = con.Query<int>("uspAnswersDeleteForCheckList", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return statusId;
        }

        public IList<QCResultsDb> GetQCResults()
        {
            IList<QCResultsDb> qCResultsDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                qCResultsDbs = con.Query<QCResultsDb>("uspQCResultsGet", commandType: CommandType.StoredProcedure).ToList();
            }
            return qCResultsDbs;
        }

        public int UpdateInspectionResult(InpsectionConclusionRequest inpsectionResultRequest)
        {
            int statusId;
            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@InspectionID", inpsectionResultRequest.InspectionID);
                param.Add("@ResultID", inpsectionResultRequest.ResultID);
                param.Add("@UserID", UserHelper.GetUserId());
                con.Open();
                statusId = con.Query<int>("uspQCInspectionResultUpdate", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return statusId;
        }

        public void UpdateInspectionCompletedFields(int inspectionId)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@InspectionID", inspectionId);
                param.Add("@CreatedBy", UserHelper.GetUserId());
                con.Open();
                con.Query("uspQCInspectionCompleteUpdate", param, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            
        }
    }
}
