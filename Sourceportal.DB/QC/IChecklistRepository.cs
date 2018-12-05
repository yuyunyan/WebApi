using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.DB.QC;

namespace Sourceportal.DB.QC
{
   public interface IChecklistRepository
    {
        IList<ChecklistDb> GetCheckList();
        ChecklistDb GetCheckListById(int checkListId);
        IList<ChecklistDb> GetCheckListType();
        IList<ChecklistDb> GetCheckListParentOptions();
        ChecklistDb SetCheckList(CheckListRequest checkListRequest);
        IList<ChecklistAssociationsDb> GetChecklistAssociations(int checklistId);
        IList<CheckListLinkTypeDb> GetChecklistAssociationsLinkTypes();
        ChecklistDb SetCheckListAssociation(ChecklistAssociationSetRequest request);
        IList<QuestionDb> GetCheckListQuestion(int checklistId);
        QuestionDb GetQuestionDetail(int questionId);
        QuestionDb SetQuestionDetail(QuestionRequest questionRequest);
        void DeleteQuestion(int QuestionID);
        void DeleteCheckListAssociation(ChecklistAssociationDeleteRequest request);
        List<AnswerTypeDb> GetQCAnswerTypes();
    }
}
