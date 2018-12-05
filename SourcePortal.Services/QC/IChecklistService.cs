using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.API.Responses.QC;
using Sourceportal.Domain.Models.API.Responses;

namespace SourcePortal.Services.QC
{
   public interface IChecklistService
    {
        ChecklistResponse GetCheckList();
        Checklist GetCheckListById(int checkListId);
        CheckListTypeResponse GetCheckListType();
        CheckListParentOptionsResponse GetCheckListParentOptions();
        CheckListSetResponse SetCheckList(CheckListRequest checkListRequest);
        CheckListAssociationsResponse GetChecklistAssociations(int checklistId);
        CheckListAssociationsLinkTypesResponse GetCheckListAssociationsLinkTypes();
        CheckListSetResponse SetCheckListAssociation(ChecklistAssociationSetRequest request);
        QuestionByCheckListResponse GetCheckListQuestion(int checklistId);
        QuestionResponse GetQuestionDetail(int questionId);
        QuestionSetResponse SetQuestionDetail(QuestionRequest questionRequest);
        void DeleteQuestion(int QuestionID);
        BaseResponse DeleteCheckListAssociation(ChecklistAssociationDeleteRequest request);
        AnswerTypesResponse GetQCAnswerTypes();
    }
}
