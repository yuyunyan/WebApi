using System.Web.Http;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.API.Responses.QC;
using SourcePortal.Services.QC;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sourceportal.API.Controllers
{
    public class QcChecklistController : ApiController
    {
        private readonly IChecklistService _checklistService;

        public QcChecklistController(IChecklistService checklistService)
        {
            _checklistService = checklistService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/getCheckList")]
        public ChecklistResponse GetCheckList()
        {
            return _checklistService.GetCheckList();
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/getCheckListById")]
        public Checklist GetCheckList(int checkListId)
        {
            return _checklistService.GetCheckListById(checkListId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/getCheckListType")]
        public CheckListTypeResponse GetCheckListType()
        {
            return _checklistService.GetCheckListType();
        }

        [HttpGet]
        [Route("api/qc-checklist/getCheckListParentOptions")]
        public CheckListParentOptionsResponse GetCheckListParentOptions()
        {
            return _checklistService.GetCheckListParentOptions();
        }
        [Authorize]
        [HttpPost]
        [Route("api/qc-checklist/setCheckList")]
        public CheckListSetResponse SetCheckList(CheckListRequest checkListRequest)
        {
            return _checklistService.SetCheckList(checkListRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/getChecklistAssociations")]
        public CheckListAssociationsResponse GetChecklistAssociations(int checklistId)
        {
            return _checklistService.GetChecklistAssociations(checklistId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/getLinkTypes")]
        public CheckListAssociationsLinkTypesResponse GetCheckListAssociationsLinkTypes()
        {
            return _checklistService.GetCheckListAssociationsLinkTypes();
        }

        [Authorize]
        [HttpPost]
        [Route("api/qc-checklist/setCheckListAssociation")]
        public CheckListSetResponse SetCheckListAssociation(ChecklistAssociationSetRequest request)
        {
            return _checklistService.SetCheckListAssociation(request);
        }

        [Authorize]
        [HttpPost]
        [Route("api/qc-checklist/deleteCheckListAssociation")]
        public BaseResponse DeleteCheckListAssociation(ChecklistAssociationDeleteRequest request)
        {
            return _checklistService.DeleteCheckListAssociation(request);
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/getChecklistQuestions")]
        public QuestionByCheckListResponse GetCheckListQuestion(int checklistId)
        {
            return _checklistService.GetCheckListQuestion(checklistId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/GetQuestionDetail")]
        public QuestionResponse GetQuestionDetail(int questionId)
        {
            return _checklistService.GetQuestionDetail(questionId);
        }

        [Authorize]
        [Authorize]
        [HttpPost]
        [Route("api/qc-checklist/SetQuestionDetail")]
        public QuestionSetResponse SetCheckListQuestion(QuestionRequest questionRequest)
        {
            return _checklistService.SetQuestionDetail(questionRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/qc-checklist/DeleteQuestion")]
        public int DeleteCheckListQuestion(QuestionDeleteRequest request)
        {
            _checklistService.DeleteQuestion(request.QuestionId);
            return 0;
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/GetAnswerTypes")]
        public AnswerTypesResponse GetQCAnswerTypes()
        {
            return _checklistService.GetQCAnswerTypes();
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-checklist/getQuestionExportList")]
        public ExportResponse GetQuestionExportList(int checkListId)
        {
            QuestionByCheckListResponse questionList = _checklistService.GetCheckListQuestion(checkListId);
            List<QuestionByCheckListResponse> questionRs = new List<QuestionByCheckListResponse>();
            {
                //questionRs = questionRs[0].QuestionsResponse;
                questionRs.Add(questionList);
            }

            //Convert to datatable (list does not work here)
            //DataTable dt = Sourceportal.Utilities.CreateExcelFile.ListToDataTable(inspectionList);

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_QuestionList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument(questionRs[0].QuestionsResponse, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

    }
}
