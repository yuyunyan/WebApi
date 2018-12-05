using System.Collections.Generic;
using System.Linq;
using SourcePortal.Services.VendorRFQs;
using System.Web.Http;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.VendorRfqs;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Comments;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using Sourceportal.Domain.Models.API.Responses.RFQ;
using SourcePortal.Services.CommonData;
using System;

namespace Sourceportal.API.Controllers
{
    public class VendorRfqController : ApiController
    {

        private readonly IVendorRfqService _rfqService;
        private readonly ICommonDataService _commonDataService;

        public VendorRfqController(IVendorRfqService rfqService, ICommonDataService commonDataService)
        {
            _rfqService = rfqService;
            _commonDataService = commonDataService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/rfqs/getBasicDetails")]
        public RfqDetailsResponse GetRfqBasicDetails(int rfqId)
        {
            return _rfqService.GetRfqBasicDetails(rfqId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/getAllRfqs")]
        public RfqListResponse GetAllRfqs(VendorRfqLineResponsesGetRequest vendorRfqRequest)
        {
           return _rfqService.GetAllRfqs(vendorRfqRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/rfqs/getAllRfqsExport")]
        public ExportResponse GetAllRfqsExport(string searchString)
        {

            //Aquire full list
            var request = new VendorRfqLineResponsesGetRequest{ SearchString = searchString, RowLimit = 999999999};
            List<RfqDetailsResponse> rfqList = _rfqService.GetAllRfqs(request).RfqList;

            //Turn list into excel
            string path = "";   //Will get transformed 
            string searchFileName = "";
            
            //Add search parameter to file name if present
            if (!string.IsNullOrEmpty(request.SearchString))
                searchFileName = "_Search_" + request.SearchString;

            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_RFQList" + searchFileName + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<RfqDetailsResponse>(rfqList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/rfqs/getRfqLines")]
        public RfqLinesResponse GetRfqLines(int rfqId = 0, int rfqLineId = 0, int rowOffset = 0, int rowLimit = 50, string sortBy = null, int descSort = 0, string partNumberStrip = null, int? statusId = null)
        {
            var vendorRfqLinesGetRequest = new VendorRfqLinesGetRequest
            {
                DescSort = descSort,
                PartNumberStrip = partNumberStrip,
                RfqId = rfqId,
                RfqLineId = rfqLineId,
                RowLimit = rowLimit,
                RowOffset = rowOffset,
                SortBy = sortBy,
                StatusId = statusId
            };

            return _rfqService.GetRfqLines(vendorRfqLinesGetRequest);
        }

        [HttpGet]
        [Route("api/rfqs/getRfqLinesExport")]
        public ExportResponse GetRfqLinesExport(int rfqId, int statusId,string partNumberStrip)
        {
            var vendorRfqLinesGetRequest = new VendorRfqLinesGetRequest
            {
                PartNumberStrip = partNumberStrip,
                RfqId = rfqId,
                StatusId = statusId,
                RowLimit = 999999999,
            };
            //Aquire full list
            List<RfqLines> list = _rfqService.GetRfqLines(vendorRfqLinesGetRequest).RfqLines.ToList();
            //Turn list into excel
            string path = "";   //Will get transformed 
            string rfqName = "";

            //Add RFQ ID to filename if present
            if (rfqId > 0)
                rfqName += "_" + rfqId;

            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_RFQLinesList" + rfqName + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<RfqLines>(list, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/rfqs/getRfqStatuses")]
        public StatusListResponse GetRfqStatuses()
        {
            return _commonDataService.GetStatusesResponse(ObjectType.VendorRfq);
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/saveBasicDetails")]
        public int SaveBasicDetails(VendorRfqSaveRequest vendorRfqSaveRequest)
        {
            return _rfqService.SaveBasicDetails(vendorRfqSaveRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/createRfqFromSourcing")]
        public int CreateRfqFromSourcing(VendorRfqCreateNewRequest request)
        {
            return _rfqService.CreateNewRfqAndLines(request);
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/sendRfqsToSuppliers")]
        public bool SendRfqsToSuppliers(RfqSendToSupplierRequest rfqSendRequest)
        {
            return _rfqService.SendRfqAndLines(rfqSendRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/saveRfqLine")]
        public RfqLines SaveRfqLine(RfqLineSaveRequest rfqLineSaveRequest)
        {
            return _rfqService.SaveRfqLine(rfqLineSaveRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/deleteRfqLines")]
        public BaseResponse DeleteRfqLines(RfqLines[] rfqLines)
        {
            return _rfqService.DeleteRfqLines(rfqLines.Select(x =>x.VRFQLineID).ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("api/rfqs/getRfqLineResponses")]
        public RfqLineResponsesResponse GetRfqLineResponses(int vRfqLineId, int rowOffset = 0, int rowLimit = 50, string sortBy = null, int descSort = 0)
        {
            var request = new VendorRfqLineResponsesGetRequest
            {
                DescSort = descSort,
                RfqLineId = vRfqLineId,
                RowLimit = rowLimit,
                RowOffset = rowOffset,
                SortBy = sortBy
            };

            return _rfqService.GetRfqLineResponses(request);
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/saveRfqLineResponse")]
        public RfqLineResponse SaveRfqLineResponse(RfqLineResponseSaveRequest rfqLineResponseSaveRequest)
        {
            return _rfqService.SaveRfqLineResponse(rfqLineResponseSaveRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/deleteRfqLineResponses")]
        public BaseResponse DeleteRfqLineResponses(RfqLineResponse[] rfqLineResponses, int rfqLineId)
        {
            return _rfqService.DeleteRfqLineResponses(rfqLineResponses.Select(x => x.SourceId).ToList(), rfqLineId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/rfqs/getRfqObjectTypeId")]
        public ObjectTypeIdResponse GetRfqObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.VendorRfq;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/rfqs/getRfqLineObjectTypeId")]
        public ObjectTypeIdResponse GetRfqLineObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.VendorRfqLine;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/rfqs/getRfqCommentTypeIds")]
        public CommentTypeIdsResponse GetRfqCommentTypeIds()
        {
            var response = new CommentTypeIdsResponse();

            var commentTypeIds = new List<CommentTypeMap>();

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.CommentRfqResponse,
                TypeName = "Comment"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.SalesRfqResponse,
                TypeName = "Sales"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.PurchaseRfqResponse,
                TypeName = "Purchasing"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.WarehouseRfqResponse,
                TypeName = "Warehouse"
            });

            response.CommentTypeIds = commentTypeIds;
            response.IsSuccess = true;
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("api/rfqs/newRfqFromFlaggedSet")]
        public RfqDetailsResponse RfqFromFlaggedSet(
            SetRfqItemsFlaggedRequest setRfqItemsFlaggedRequest)
        {
            return _rfqService.RfqFromFlaggedSet(setRfqItemsFlaggedRequest);
        }

    }
}