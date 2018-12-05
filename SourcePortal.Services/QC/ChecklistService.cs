using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.QC;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.API.Responses.QC;
using Sourceportal.Domain.Models.API.Responses;

namespace SourcePortal.Services.QC
{
   public class ChecklistService:IChecklistService
   {
       private readonly IChecklistRepository _checklistRepository;

       public ChecklistService(IChecklistRepository checklistRepository)
       {
           _checklistRepository = checklistRepository;
       }

        public ChecklistResponse GetCheckList()
       {
           var dbChecklist = _checklistRepository.GetCheckList();
           var checkLists = new List<Checklist>();

           foreach (var value in dbChecklist)
           {
               if (value.ParentChecklistId == 0)
               {
                   checkLists.Add(new Checklist
                   {
                       ChecklistId = value.ChecklistId,
                       ParentChecklistId = value.ParentChecklistId,
                       ChecklistName = value.ChecklistName,
                       ChecklistDescription = value.ChecklistDescription,
                       ChecklistTypeId = value.ChecklistTypeId,
                       TypeName = value.ChecklistTypeName,
                       SortOrder = value.SortOrder,
                       EffectiveStartDate = value.EffectiveStartDate,
                       IsDeleted = value.IsDeleted,
                       ChildCheckList = new List<Checklist>()

                   });
                }
           }

            foreach (var value in dbChecklist)
            {
                if (value.ParentChecklistId != 0)
                {
                    var parentCheckList = checkLists.First(x => x.ChecklistId == value.ParentChecklistId);
                    parentCheckList.ChildCheckList.Add(new Checklist
                    {
                        ChecklistId = value.ChecklistId,
                        ParentChecklistId = value.ParentChecklistId,
                        ChecklistName = value.ChecklistName,
                        ChecklistDescription = value.ChecklistDescription,
                        ChecklistTypeId = value.ChecklistTypeId,
                        TypeName = value.ChecklistTypeName,
                        SortOrder = value.SortOrder,
                        EffectiveStartDate = value.EffectiveStartDate,
                        IsDeleted = value.IsDeleted

                    });
                }
            }

            return new ChecklistResponse{CheckLists = checkLists };
       }

       public Checklist GetCheckListById(int checkListId)
       {
           var dbCheckList = _checklistRepository.GetCheckListById(checkListId);
           var list = new Checklist();
           list.ParentChecklistId = dbCheckList.ParentChecklistId;
           list.ChecklistDescription = dbCheckList.ChecklistDescription;
           list.ChecklistId = dbCheckList.ChecklistId;
           list.ChecklistName = dbCheckList.ChecklistName;
           list.ChecklistTypeId = dbCheckList.ChecklistTypeId;
           list.TypeName = dbCheckList.ChecklistTypeName;
           list.EffectiveStartDate = dbCheckList.EffectiveStartDate.Split(' ')[0];
           list.SortOrder = dbCheckList.SortOrder;
           list.IsDeleted = dbCheckList.IsDeleted;
           return list;
       }

       public CheckListTypeResponse GetCheckListType()
       {
           var dbCheckListType = _checklistRepository.GetCheckListType();
           var response = new List<CheckListType>();
           foreach (var value in dbCheckListType)
           {
               response.Add(new CheckListType
               {
                   TypeName = value.ChecklistTypeName,
                   TypeId = value.ChecklistTypeId
               });
           }
           return new CheckListTypeResponse{CheckListTypes = response};
       }

       public CheckListParentOptionsResponse GetCheckListParentOptions()
       {
           var dbParentOptions = _checklistRepository.GetCheckListParentOptions();
           var response = new List<ParentOptonsResponse>();
           foreach (var value in dbParentOptions)
           {
               response.Add(new ParentOptonsResponse
               {
                   Id = value.ChecklistId,
                   Name = value.ChecklistName
               });
           }
           return new CheckListParentOptionsResponse{CheckListParent = response};
       }

       public CheckListSetResponse SetCheckList(CheckListRequest checkListRequest)
       {
           var dbSetCheckList = _checklistRepository.SetCheckList(checkListRequest);
           var response = new CheckListSetResponse();
           response.ChecklistId = dbSetCheckList.ChecklistId;
           return response;
       }

        public CheckListAssociationsResponse GetChecklistAssociations(int checklistId)
        {
            var dbGetAssociations = _checklistRepository.GetChecklistAssociations(checklistId);
            var response = new List<CheckListAssociation>();
            foreach (var value in dbGetAssociations)
            {
                response.Add(new CheckListAssociation
                {
                    LinkType = value.LinkType,
                    Value = value.Value,
                    ObjectID = value.ObjectID,
                    ObjectTypeID = value.ObjectTypeID

                });
            }
            return new CheckListAssociationsResponse { ChecklistAssociations = response };
        }

        public CheckListAssociationsLinkTypesResponse GetCheckListAssociationsLinkTypes()
        {
            var dbGetLinkTypes = _checklistRepository.GetChecklistAssociationsLinkTypes();
            var response = new List<ChecklistAssociationsLinkType>();
            foreach (var value in dbGetLinkTypes)
            {
                response.Add(new ChecklistAssociationsLinkType
                {
                    ObjectName = value.ObjectName,
                    ObjectTypeID = value.ObjectTypeID,
                    AccountTypeID = value.AccountTypeID

                });
            }
            return new CheckListAssociationsLinkTypesResponse { ChecklistAssociationsLinkTypes = response };

        }

        public CheckListSetResponse SetCheckListAssociation(ChecklistAssociationSetRequest request)
        {
            var dbSetCheckList = _checklistRepository.SetCheckListAssociation(request);
            var response = new CheckListSetResponse();
            response.ChecklistId = dbSetCheckList.ChecklistId;
            return response;
        }

        public BaseResponse DeleteCheckListAssociation(ChecklistAssociationDeleteRequest request)
        {
            _checklistRepository.DeleteCheckListAssociation(request);
            return new BaseResponse{ IsSuccess = true };
        }

       public QuestionByCheckListResponse GetCheckListQuestion(int checklistId)
       {
           var dbGetCheckListQuestion = _checklistRepository.GetCheckListQuestion(checklistId);
           var list = new List<QuestionResponse>();
           foreach (var value in dbGetCheckListQuestion)
           {
               list.Add(new QuestionResponse
               {
                   CheckListId = value.CheckListId,
                   QuestionId = value.QuestionId,
                   VersionId = value.VersionId,
                   AnswerTypeId = value.AnswerTypeId,
                   AnswerTypeName = value.AnswerTypeName,
                   SortOrder = value.SortOrder,
                   QuestionText = value.QuestionText,
                   QuestionSubText = value.QuestionSubText,
                   QuestionHelpText = value.QuestionHelpText,
                   CanComment = value.CanComment,
                   PrintOnInspectReport = value.PrintOnInspectReport,
                   RequiresPicture = value.RequiresPicture,
                   PrintOnRejectReport = value.PrintOnRejectReport,
                   RequireSignature = value.RequiresSignature,
                   IsDeleted = value.IsDeleted,
                   TotalRows = value.TotalRows
               });
           }
           return new QuestionByCheckListResponse{QuestionsResponse = list};
       }

       public QuestionResponse GetQuestionDetail(int questionId)
       {
           var dbGetQuestionDetail = _checklistRepository.GetQuestionDetail(questionId);
           var response = new QuestionResponse();
           response.CheckListId = dbGetQuestionDetail.CheckListId;
           response.QuestionId = dbGetQuestionDetail.QuestionId;
           response.VersionId = dbGetQuestionDetail.VersionId;
           response.AnswerTypeId = dbGetQuestionDetail.AnswerTypeId;
           response.AnswerTypeName = dbGetQuestionDetail.AnswerTypeName;
           response.SortOrder = dbGetQuestionDetail.SortOrder;
           response.QuestionText = dbGetQuestionDetail.QuestionText;
           response.QuestionSubText = dbGetQuestionDetail.QuestionSubText;
           response.QuestionHelpText = dbGetQuestionDetail.QuestionHelpText;
           response.CanComment = dbGetQuestionDetail.CanComment;
           response.PrintOnInspectReport = dbGetQuestionDetail.PrintOnInspectReport;
           response.RequiresPicture = dbGetQuestionDetail.RequiresPicture;
           response.PrintOnRejectReport = dbGetQuestionDetail.PrintOnRejectReport;
           response.RequireSignature = dbGetQuestionDetail.RequiresSignature;
           response.IsDeleted = dbGetQuestionDetail.IsDeleted;
           return response;
       }

       public QuestionSetResponse SetQuestionDetail(QuestionRequest questionRequest)
       {
           var dbSetQuestionDetail = _checklistRepository.SetQuestionDetail(questionRequest);
           var response = new QuestionSetResponse();
           response.QuestionId = dbSetQuestionDetail.QuestionId;
           response.VersionId = dbSetQuestionDetail.VersionId;
           return response;
       }

        public void DeleteQuestion(int QuestionID)
        {
            _checklistRepository.DeleteQuestion(QuestionID);
        }

        public AnswerTypesResponse GetQCAnswerTypes()
       {
           var response = new AnswerTypesResponse();
           var answerType = new List<TypeResponse>();
           var dbAnswerTypes = _checklistRepository.GetQCAnswerTypes();
           foreach (var dbAnswerType in dbAnswerTypes)
           {
               var typeResponse = new TypeResponse
               {
                   AnswerTypeID = dbAnswerType.AnswerTypeID,
                   TypeName = dbAnswerType.TypeName
               };
               answerType.Add(typeResponse);
           }
           response.AnswerTypes = answerType;

           return response;
       }
   }
}
