using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.DB.QC;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;

namespace Sourceportal.DB.QC
{
   public class ChecklistRepository:IChecklistRepository
    {
        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public IList<ChecklistDb> GetCheckList()
        {
            IList<ChecklistDb> checkListDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                checkListDbs =  con.Query<ChecklistDb>("uspQCChecklistsGet", commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return checkListDbs;

        }

        public ChecklistDb GetCheckListById(int checkListId)
        {
            ChecklistDb checkListDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChecklistID", checkListId);
                var res = con.Query<ChecklistDb>("uspQCChecklistsGet", param, commandType: CommandType.StoredProcedure);

                if (res.Count() != 1)
                {
                    var errorMessage = string.Format("Getting checklist id# {0} failed.", checkListId);
                    throw new GlobalApiException(errorMessage);
                }

                checkListDbs = res.First();
                con.Close();
            }
            return checkListDbs;
        }

        public IList<ChecklistDb> GetCheckListType()
        {
            IList<ChecklistDb> checkListDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                checkListDbs = con.Query<ChecklistDb>("uspQCChecklistTypeGet", commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return checkListDbs;
        }

        public IList<ChecklistDb> GetCheckListParentOptions()
        {
            IList<ChecklistDb> checkListDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                checkListDbs = con.Query<ChecklistDb>("uspQCParentChecklistOptionsGet", commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return checkListDbs;
        }

        public ChecklistDb SetCheckList(CheckListRequest checkListRequest)
        {
            ChecklistDb checkListDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChecklistID", checkListRequest.ChecklistId);
                param.Add("@ParentChecklistID",checkListRequest.ParentChecklistId);
                param.Add("@ChecklistTypeID", checkListRequest.ChecklistTypeId);
                param.Add("@ChecklistName", checkListRequest.ChecklistName);
                param.Add("@ChecklistDescription", checkListRequest.ChecklistDescription);
                param.Add("@SortOrder",checkListRequest.SortOrder);
                param.Add("@EffectiveStartDate", checkListRequest.EffectiveStartDate);
                param.Add("@IsDeleted", checkListRequest.IsDeleted);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspQCChecklistSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", ChecklistDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                param = new DynamicParameters();
                param.Add("@ChecklistID", res.First().ChecklistId);

                checkListDbs = con.Query<ChecklistDb>("uspQCChecklistsGet", param,commandType: CommandType.StoredProcedure)
                    .First();
                 
                con.Close();
            }
            return checkListDbs;
        }

        public IList<ChecklistAssociationsDb> GetChecklistAssociations(int checklistId)
        {
            IList<ChecklistAssociationsDb> associationsDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChecklistID", checklistId);
                associationsDbs = con.Query<ChecklistAssociationsDb>("uspQCChecklistAssociationsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return associationsDbs;
        }

        public IList<QuestionDb> GetCheckListQuestion(int checklistId)
        {
            IList<QuestionDb> questionDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChecklistID", checklistId);
                questionDbs = con.Query<QuestionDb>("uspQCCheckListQuestionGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return questionDbs;
        }

        public QuestionDb GetQuestionDetail(int questionId)
        {
            QuestionDb questionDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@QuestionID", questionId);
                questionDbs = con.Query<QuestionDb>("uspQCQuestionGetbyQuestionId", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return questionDbs;
        }

        public void DeleteQuestion(int QuestionID)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@QuestionID", QuestionID);

                var res = con.Query("uspQCCheckListQuestionDeleteSet", param,
                    commandType: CommandType.StoredProcedure);
            }
        }
        public QuestionDb SetQuestionDetail(QuestionRequest questionRequest)
        {
            QuestionDb questionDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@QuestionID", questionRequest.QuestionId);
                param.Add("@ChecklistID",questionRequest.CheckListId);
                param.Add("@VersionID",questionRequest.VersionId);
                param.Add("@AnswerTypeID",questionRequest.AnswerTypeId);
                param.Add("@SortOrder",questionRequest.SortOrder);
                param.Add("@QuestionText",questionRequest.QuestionText);
                param.Add("@QuestionSubText",questionRequest.QuestionSubText);
                param.Add("@QuestionHelpText",questionRequest.QuestionHelpText);
                param.Add("@CanComment",questionRequest.CanComment);
                param.Add("@RequiresPicture",questionRequest.RequiresPicture);
                param.Add("@PrintOnInspectReport",questionRequest.PrintOnInspectReport);
                param.Add("@PrintOnRejectReport",questionRequest.PrintOnRejectReport);
                param.Add("@RequiresSignature",questionRequest.RequireSignature);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@IsDeleted",questionRequest.IsDeleted);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspQCCheckListQuestionSet", param,
                    commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", ChecklistDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                param = new DynamicParameters();
                param.Add("@QuestionID", res.First().QuestionId);

                questionDbs = con.Query<QuestionDb>("uspQCQuestionGetbyQuestionId", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return questionDbs;
        }

   public IList<CheckListLinkTypeDb> GetChecklistAssociationsLinkTypes()
        {
            IList<CheckListLinkTypeDb> linkTypesDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                linkTypesDb = con.Query<CheckListLinkTypeDb>("uspQCChecklistAssociationsLinkTypesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return linkTypesDb;
        }

        public ChecklistDb SetCheckListAssociation(ChecklistAssociationSetRequest request)
        {
            ChecklistDb checkListDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChecklistID", request.ChecklistId);
                param.Add("@ObjectID", request.ObjectID);
                param.Add("@ObjectTypeID", request.ObjectTypeID);
                param.Add("@CreatedBy", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<ChecklistDb>("uspQCChecklistAssociationsSet", param,
                    commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", ChecklistDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                checkListDbs = res.First();
                con.Close();
            }

            return checkListDbs;
        }

        public void DeleteCheckListAssociation(ChecklistAssociationDeleteRequest request)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChecklistID", request.ChecklistId);
                param.Add("@ObjectID", request.ObjectID);
                param.Add("@ObjectTypeID", request.ObjectTypeID);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                con.Query("uspQCChecklistAssociationsDelete", param,
                    commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", ChecklistDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                
                con.Close();
            }
        }

        public List<AnswerTypeDb> GetQCAnswerTypes()
        {
            List<AnswerTypeDb> answerTypeDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                answerTypeDbs = con.Query<AnswerTypeDb>("uspQCAnswerTypesGet", commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }

            return answerTypeDbs;
        }
        
    }
}
