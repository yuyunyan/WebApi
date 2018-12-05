using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sourceportal.Domain.Models.API.Requests;
using System.Web.Http;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses.Sourcing;
using Sourceportal.Domain.Models.API.Requests.Sourcing;
using Sourceportal.Domain.Models.API.Responses;
using SourcePortal.Services.Sourcing;
using Sourceportal.Domain.Models.API.Responses.CommonData;

namespace Sourceportal.API.Controllers
{
    public class SourcingController : ApiController
    {
        private ISourcingService _sourcingService;

        public SourcingController(ISourcingService sourcingService)
        {
            _sourcingService = sourcingService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getSourcingQuoteLinesList")]
        public SourcingQuoteLinesListResponse SourcingQuoteLinesList(int status, int rowOffset, int rowLimit, string sortCol, bool descSort, string filterBy, string filterText)
        {
            SourcingQuoteLinesFilter sourcefilter = new SourcingQuoteLinesFilter
            {
                StatusId = status,
                RowOffset = rowOffset,
                RowLimit = rowLimit,
                SortCol = sortCol,
                DescSort = descSort,
                FilterBy = filterBy,
                FilterText =  filterText
            };
            return _sourcingService.GetSourcingQuoteLines(sourcefilter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getSourcingQuoteExportList/")]
        public ExportResponse GetQuoteExportList(int statusId)
        {
            //Aquire full list
            SourcingQuoteLinesFilter searchfilter = new SourcingQuoteLinesFilter
            { RowLimit = 999999999,
              StatusId = statusId,
            };
            List<SourcingQuoteLinesResponse> srcList = _sourcingService.GetSourcingQuoteLines(searchfilter).SourcingQuoteLinesList;

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_SourcingQuoteList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<SourcingQuoteLinesResponse>(srcList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;

        }
        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getStatuses")]
        public SourcingStatusesListResponse SourcingStatusesList()
        {
            return _sourcingService.GetSourcingStatuses();
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getTypes")]
        public SourceTypesListResponse SourceTypes()
        {
            return _sourcingService.GetSourceTypes();
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getSourceList")]
        public SourceListResponse GetSourceList(int itemId, string partNumber, int objectId, int objectTypeId, bool showAll,bool showInventory)
        {
            return _sourcingService.GetSourceList(itemId, partNumber, objectId, objectTypeId , showAll, showInventory);
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getSourceExportList")]
        public ExportResponse GetSourceExportList(int itemId, string partNumber, int objectId, int objectTypeId)
        {

            //Aquire full list
            List<SourceResposne> srcList = _sourcingService.GetSourceList(itemId, partNumber, objectId, objectTypeId, true, true).SourceResponse;

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_PartSources.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<SourceGridExportLine>(_sourcingService.MapSourceLinesToExport(srcList), ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpPost]
        [Route("api/sourcing/setSource")]
        public SetSourceResponse SetSource(SetSourceRequest setSourceRequest)
        {
            return _sourcingService.SetSource(setSourceRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/sourcing/setSourceStatus")]
        public BaseResponse SetSourceStatus(SetSourceStatus setSourceStatus)
        {
            return _sourcingService.SetSourceStatus(setSourceStatus);
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getSourceObjectTypeId")]
        public ObjectTypeIdResponse GetSourceObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int) ObjectType.Source;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getSourcesJoinObjectTypeId")]
        public ObjectTypeIdResponse GetSourcesJoinObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.SourcesJoin;
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("api/sourcing/getSourcesJoinCommentUId")]
        public SourceCommentUIDResponse GetSourceCommentUID(SourceCommentUIDRequest sourceCommentUidRequest)
        {
            return _sourcingService.GetSourceCommentUID(sourceCommentUidRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getRouteStatuses")]
        public SourcingRouteStatusesResponse GetRouteStatuses()
        {
            return _sourcingService.GetRouteSatatuses();
        }

        [Authorize]
        [HttpPost]
        [Route("api/sourcing/setBuyerRoutes")]
        public BaseResponse SetBuyerRoutes(SetBuyerRouteRequest setBuyerRouteRequest)
        {
            return _sourcingService.SetBuyerRoutes(setBuyerRouteRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getQuoteLineBuyers")]
        public QuoteLineBuyersGetResponse GetQuoteLineBuyers(int quoteLineId)
        {
            return _sourcingService.QuoteLineBuyersGet(quoteLineId);
        }


        [Authorize]
        [HttpPost]
        [Route("api/sourcing/SourceToPurchaseOrder")]
        public BaseResponse SourceToPurchaseOrder(SourceToPORequest request)
        {
            return _sourcingService.SourceToPurchaseOrder(request);
        }


        [Authorize]
        [HttpGet]
        [Route("api/sourcing/RTPSourceExport")]
        public ExportResponse RTPSourceExport(float soPrice, int itemId, string partNumber, int objectId, int objectTypeId)
        {
            List<SourceResposne> srcList = _sourcingService.GetSourceList(itemId, partNumber, objectId, objectTypeId, false, true).SourceResponse.Where(s => s.TypeName != "Inventory").ToList();

            string path = "";   
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_RTPSources.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<RTPSourceExportLine>(_sourcingService.MapRTPSourceLinesToExport(srcList, soPrice), ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            if (export.Success)
                export.DownloadURL = path.Substring(1); 
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/sourcing/getSourceLineByAccountId")]
        public SourceHistoryResponse GetSourceLineByAccountId(int accountId, int? contactId=null)
        {
            return _sourcingService.GetSourceLineByAccountId(accountId, contactId);
        }
    }
}