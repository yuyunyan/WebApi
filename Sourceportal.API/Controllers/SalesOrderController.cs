using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses.SalesOrders;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Comments;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using SourcePortal.Services.CommonData;
using SourcePortal.Services.SalesOrder;
using System;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Utilities;


namespace Sourceportal.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SalesOrderController : ApiController
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ICommonDataService _commonDataService;

        public SalesOrderController(ISalesOrderService salesOrderService, ICommonDataService commonDataService)
        {
            _salesOrderService = salesOrderService;
            _commonDataService = commonDataService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/sales-order/setSalesOrderDetails")]
        public SalesOrderDetailsResponse SetSalesOrderDetails(SalesOrderDetailsRequest request)
        {
            return _salesOrderService.SetSalesOrderDetails(request);
        }

       // [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderDetails")]
        public SalesOrderDetailsResponse GetSalesOrderDetails(int soId, int versionId)
        {
            return _salesOrderService.GetSalesOrderDetails(soId, versionId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/sales-order/getSalesOrderList")]
        public SalesOrderListResponse SalesOrderList(SearchFilter searchFilter)
        {
            return _salesOrderService.GetSalesOrderList(searchFilter);
        }


        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderExportList")]
        public ExportResponse SalesOrderExportList(string searchString)
        {
            //Aquire full list
            SearchFilter searchfilter = new SearchFilter { RowLimit = 999999999, SearchString = searchString };
            List<SalesOrderResponse> srcList = _salesOrderService.GetSalesOrderList(searchfilter).SalesOrders.ToList();

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_SalesOrdersList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<SalesOrderResponse>(srcList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderLines")]
        public GetSalesOrderLinesResponse GetSalesOrderLines(int soId, int soVersionId, int rowOffset=0, int rowLimit=100000, string sortCol="", bool descSort=false)
        {
            SearchFilter searchfilter = new SearchFilter
            {
                SearchString = "",
                RowOffset = rowOffset,
                RowLimit = rowLimit,
                SortCol = sortCol,
                DescSort = descSort
            };
            return _salesOrderService.GetSalesOrderLines(soId, soVersionId, searchfilter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderExportLines")]
        public ExportResponse getSalesOrderExportLines(int soId, int soVersionId)
        {

            //Aquire full list
            SearchFilter searchfilter = new SearchFilter { RowLimit = 999999999 };
            List<SalesOrderLineDetail> srcList = _salesOrderService.GetSalesOrderLines(soId, soVersionId, searchfilter).SOLinesResponse.ToList();

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_SalesOrder[" + soId + "]V[" + soVersionId + "]_PartsList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<SalesOrderLineDetail>(srcList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }
        
        [Authorize]
        [HttpPost]
        [Route("api/sales-order/setSalesOrderLine")]
        public SalesOrderLineDetail SetSalesOrderLines(SalesOrderLineDetail setSalesOrderLinesRequest)
        {
            return _salesOrderService.SetSalesOrderLines(setSalesOrderLinesRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/sales-order/deleteSalesOrderLines")]
        public SalesOrderLinesDeleteResponse DeleteSalesOrderLines(SalesOrderLineDetail[] soLines)
        {
            return _salesOrderService.DeleteSalesOrderLines(soLines.ToList().Select(x => x.SOLineId).ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderExtra")]
        public SalesOrderExtraResponse GetSalesOrderExtra(int soId, int soVersionId, int rowOffset, int rowLimit)
        {
            return _salesOrderService.GetSalesOrderExtra(soId, soVersionId, rowOffset, rowLimit);
        }


        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderExtraExport")]
        public ExportResponse GetSalesOrderExtraExport(int soId, int soVersionId)
        {
            //Aquire full list
            List<ExtraListResponse> srcList = _salesOrderService.GetSalesOrderExtra(soId, soVersionId, 0, 999999999).ExtraListResponse.ToList();

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_SalesOrder[" + soId + "]V[" + soVersionId + "]_ExtraList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<ExtraListResponse>(srcList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;

        }

        [Authorize]
        [HttpPost]
        [Route("api/sales-order/setSalesOrderExtra")]
        public SetSalesOrderExtraResponse SetSalesOrderExtra(SetSalesOrderExtraRequest setSalesOrderExtraRequest)
        {
            return _salesOrderService.SetSalesOrderExtra(setSalesOrderExtraRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/sales-order/deleteSalesOrderExtras")]
        public SalesOrderExtrasDeleteResponse DeleteSalesOrderExtras(ExtraListResponse[] soExtras)
        {
            return _salesOrderService.DeleteSalesOrderExtras(soExtras.ToList().Select(x => x.SOExtraId).ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getStatusBySalesOrder")]
        public StatusListResponse GetStatusBySalesOrder()
        {
            return _commonDataService.GetStatusesResponse(ObjectType.Salesorder);
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderObjectTypeId")]
        public ObjectTypeIdResponse GetSalesOrderObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int) ObjectType.Salesorder;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSOLineObjectTypeId")]
        public ObjectTypeIdResponse GetSOLineObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int) ObjectType.SalesorderDetail;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSOExtraObjectTypeId")]
        public ObjectTypeIdResponse GetSOExtraObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.SalesorderExtra;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderCommentTypeIds")]
        public CommentTypeIdsResponse GetSOCommentTypeIds()
        {
            var response = new CommentTypeIdsResponse();

            var commentTypeIds = new List<CommentTypeMap>();

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.SalesSalesOrder,
                TypeName = "Sales"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.PurchasingSalesOrder,
                TypeName = "Purchasing"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.WarehouseSalesOrder,
                TypeName = "Warehouse"
            });

            response.CommentTypeIds = commentTypeIds;
            response.IsSuccess = true;
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("api/sales-order/sync")]
        public SyncResponse SyncSalesOrder(int soId, int soVersionId)
        {
            return _salesOrderService.Sync(soId, soVersionId);
        }

        //[Authorize]
        [HttpPost]
        [Route("api/sales-order/updateExternalId")]
        public BaseResponse UpdateExternalId(SetSalesOrderSapDataRequest request)
        {
            UserHelper.SetMiddlewareUser();

            _salesOrderService.SetSalesOrderDetailsFromSap(
                new SalesOrderDetailsRequest{SalesOrderId = request.ObjectId, ExternalId = request.ExternalId, VersionID = request.VersionId, Lines = request.Lines, Items = request.Items });

            return new BaseResponse();
        }

        

        [HttpPost]
        [Route("api/sales-order/updateSalesOrder")]
        public BaseResponse UpdateSalesOrderFromSap(SalesOrderIncomingSapResponse request)
        {
            UserHelper.SetMiddlewareUser();

            return _salesOrderService.HandlingIncomingSalesOrderSapUpdate(request);

        }

        [Authorize]
        [HttpGet]
        [Route("api/sales-order/getSalesOrderLineByAccountId")]
        public SalesOrderLineListResponse GetSalesOrderLineByAccountId(int accountId, int? contactId=null) 
        {
            return _salesOrderService.GetSalesOrderLineByAccountId(accountId, contactId);
        }
    }

}
