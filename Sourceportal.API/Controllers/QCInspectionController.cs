using Sourceportal.Domain.Models.API.Responses.QC;
using SourcePortal.Services.QC;
using System.Web.Http;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.API.Responses;
using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using System;
using System.Linq;
using System.Data;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.API.Requests.ItemStock;
using Telerik.Reporting;
using System.Web;

namespace Sourceportal.API.Controllers
{
    public class QcInspectionController : ApiController
    {
        private readonly IInspectionService _inspectionService;

        public QcInspectionController(IInspectionService inspectionService)
        {
            _inspectionService = inspectionService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/qc-inspection/getInspectionDetails")]
        public InspectionDetailsResponse GetInspectionDetails(int inspectionId)
        {
            return _inspectionService.GetInspectionDetails(inspectionId);
        }  
        
        [Authorize]
        [HttpGet]
        [Route("api/qc-inspection/getInspectionConclusion")]
        public InspectionConclusionResponse GetInspectionConclusion(int inspectionid)
        {
            return _inspectionService.GetInspectionConclusion(inspectionid);
        }

        [Authorize]
        [HttpPost]
        [Route("api/qc-inspection/setInspectionConclusion")]
        public int? SetInspectionConclusion(InpsectionConclusionRequest request)
        {
            return _inspectionService.SetInspectionConclusion(request);
        }

        [HttpGet]
        [Route("api/inspection/getCheckLists")]
        public InspectionCheckListsResponse GetCheckListsForInspection(int inspectionId)
        {
            return _inspectionService.GetInspectionCheckLists(inspectionId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/inspection/saveAnswer")]
        public BaseResponse GetCheckListsForInspection(SaveAnswerRequest saveAnswerRequest)
        {
            return _inspectionService.SaveAnswer(saveAnswerRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/inspection/getInspectionList")]
        public InspectionGridResponse GetInspectionList(string searchString,int rowOffset, int rowLimit, string sortCol, bool descSort)
        {
            return _inspectionService.GetInspections(searchString,rowOffset, rowLimit, sortCol, descSort);
        }

        [Authorize]
        [HttpGet]
        [Route("api/inspection/getInspectionExportList")]
        public ExportResponse GetInspectionExportList()
        {
            //Convert to datatable (list does not work here)
            List<InspectionGridItem> inspectionList = _inspectionService.GetInspections(null,0, 999999999, null, false).InspectionList.ToList();
            //DataTable dt = Sourceportal.Utilities.CreateExcelFile.ListToDataTable(inspectionList);

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_InspectionList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument(inspectionList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [HttpPost]
        [Route("api/inspection/set")]
        public BaseResponse InspectionSetFromSap(SetInspectionFromSapRequest request)
        {
            UserHelper.SetMiddlewareUser();
            return _inspectionService.SetInspection(request);
        }

        [HttpPost]
        [Route("api/inspection/complete")]
        public BaseResponse InspectionCompleteFromSap(SetInspectionFromSapRequest request)
        {
            UserHelper.SetMiddlewareUser();
            return _inspectionService.SetInspection(request);
        }

        //[Authorize]
        [HttpPost]
        [Route("api/qcinspection/sync")]
        public SyncResponse SyncQcInspection(int inspectionId)
        {
            UserHelper.SetMiddlewareUser();
            return _inspectionService.SyncInspectionConclusion(inspectionId);

        }

        [HttpGet]
        [Route("api/inspection/getAvailableCheckLists")]
        public InspectionCheckListsResponse GetAllCheckLists(int inspectionId)
        {
            return _inspectionService.GetAvailableCheckLists(inspectionId);
        }
        
        [HttpPost]
        [Route("api/inspection/saveChecklistForInpection")]
        public QCanswersResponse SaveChecklistForInpection(InsertChecklistForInspectionRequest questionAnswersInsertRequest)
        {
            return _inspectionService.SaveChecklistForInpection(questionAnswersInsertRequest);
        }

        [HttpPost]
        [Route("api/inspection/deleteChecklistForInpection")]
        public BaseResponse DeleteChecklistForInpection(InsertChecklistForInspectionRequest questionAnswersInsertRequest)
        {
            return _inspectionService.DeleteChecklistForInpection(questionAnswersInsertRequest);
        }

        [HttpGet]
        [Route("api/inspection/getStockWithBreakdownsList")]
        public ItemStockWithBreakdownsListResponse GetStockWithBreakdownsList(int inspectionId)
        {
            return _inspectionService.GetStockListForInspection(inspectionId);
        }

        [HttpPost]
        [Route("api/inspection/setItemStock")]
        public int SetItemStockOnInspection(SetItemStockRequest itemStock)
        {
            return _inspectionService.SetItemStockOnInspection(itemStock);
        }

        [HttpPost]
        [Route("api/inspection/deleteItemStock")]
        public int DeleteItemStockOnInspection(SetItemStockRequest itemStock)
        {
            return _inspectionService.DeleteItemStockOnInspection(itemStock.StockID);
        }
        [HttpPost]
        [Route("api/inspection/setItemStockBreakdown")]
        public int SetItemStockBreakdownOnInspection(SetItemStockBreakdownRequest itemStockBreakdown)
        {
            return _inspectionService.SetItemStockBreakdownOnInspection(itemStockBreakdown);
        }

        [HttpGet]
        [Route("api/inspection/getQCResults")]
        public QCResultsResponse GetQCResults()
        {
            return _inspectionService.GetQCResults();
        }

        [HttpPost]
        [Route("api/inspection/updateQCResult")]
        public BaseResponse UpdateQCInspectionResult(InpsectionConclusionRequest inpsectionResultRequest)
        {
            return _inspectionService.UpdateInspectionResult(inpsectionResultRequest);
        }
        [HttpGet]
        [Route("api/inspection/exportInspectionReport")]
        public string ExportInspectionReport(int inspectionId, int acceptedDiscrepant, int rejectedDiscrepant, int qtyFailed, int qtyPassed, string apiUrl)
        {
            string reportAssembly = "Sourceportal.Reports.Inspection, Sourceportal.Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            Parameter[] paramList = new Parameter[7];
            paramList[0] = new Parameter()
            {
                Name = "InspectionID",
                Value = inspectionId
            };
            paramList[1] = new Parameter()
            {
                Name = "UserID",
                Value = UserHelper.GetUserId()
            };
            paramList[2] = new Parameter()
            {
                Name = "AcceptedDiscrepantCount",
                Value = acceptedDiscrepant
            };
            paramList[3] = new Parameter()
            {
                Name = "RejectedDiscrepantCount",
                Value = rejectedDiscrepant
            };
            paramList[4] = new Parameter()
            {
                Name = "QtyPassedCount",
                Value = qtyPassed
            };
            paramList[5] = new Parameter()
            {
                Name = "QtyFailedCount",
                Value = qtyFailed
            };
            paramList[6] = new Parameter()
            {
                Name = "ApiUrl",
                Value = apiUrl
            };

            return Reports.Utilities.ExportReport(reportAssembly, inspectionId.ToString(), paramList);
        }

        [HttpGet]
        [Route("api/inspection/exportConclusionReport")]
        public string ExportConclusionReport(int inspectionId)
        {
            string reportAssembly = "Sourceportal.Reports.InspectionIdentifiedStockContainer, Sourceportal.Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            Parameter[] paramList = new Parameter[1];
            paramList[0] = new Parameter()
            {
                Name = "InspectionID",
                Value = inspectionId
            };

            return Reports.Utilities.ExportReport(reportAssembly, inspectionId.ToString(), paramList);
        }

        [HttpGet]
        [Route("api/inspection/exportFinalInspectionReport")]
        public string ExportFinalInspectionReport(int inspectionId, int acceptedDiscrepant, int rejectedDiscrepant, int qtyFailed, int qtyPassed, string apiUrl)
        {
            string[] reportPaths = new string[2];
            string outputPath = null;

            //prepare reports
            reportPaths[0] = HttpContext.Current.Server.MapPath("~/" + ExportInspectionReport(inspectionId, acceptedDiscrepant, rejectedDiscrepant, qtyFailed, qtyPassed, apiUrl));
            reportPaths[1] = HttpContext.Current.Server.MapPath("~/" + ExportConclusionReport(inspectionId));

            //merge reports
            int ret = Reports.Utilities.MergePDF(reportPaths, "Inspection_" + inspectionId + "_Merged", out outputPath);

            return outputPath;
        }
    }
}
