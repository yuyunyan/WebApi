using System.Web.Http;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.OrderFulfillment;
using Sourceportal.Domain.Models.API.Responses.OrderFulfillment;
using SourcePortal.Services.OrderFulfillment;
using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using System.Linq;
using System;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Utilities;


namespace Sourceportal.API.Controllers
{
    public class OrderFulfillmentController : ApiController 
    {
        private readonly IOrderFulfillmentService _orderFulfillmentService;

        public OrderFulfillmentController(IOrderFulfillmentService orderFulfillmentService)
        {
            _orderFulfillmentService = orderFulfillmentService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/orderFulfillment/getOrderFulfillmentList")]
        public OrderFulfillmentListResponse GetOrderFulfillmentList([FromUri] OrderFulfillmentListSearchFilter searchFilter)
        {
            return _orderFulfillmentService.GetOrderFulfillmentList(searchFilter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/orderFulfillment/getRequestToPurchaseList")]
        public OrderFulfillmentListResponse GetRequestToPurchaseList([FromUri] RequestToPurchaseListRequest searchFilter)
        {
            return _orderFulfillmentService.RequestToPurchaseListGet(searchFilter);
        }


        [Authorize]
        [HttpGet]
        [Route("api/orderFulfillment/getUnallocatedSOLines")]
        public SOAllocationListResponse GetUnallocatedSOLines([FromUri] UnallocatedSOLinesGetRequest req)
        {
            return _orderFulfillmentService.GetUnallocatedSOLines(req);
        }

        [Authorize]
        [HttpGet]
        [Route("api/orderFulfillment/getOrderFulfillmentExportList")]
        public ExportResponse GetOrderFulfillmentExportList(string searchString, bool underAllocatedOnly)
        {
            //Aquire full list
            OrderFulfillmentListSearchFilter searchfilter = new OrderFulfillmentListSearchFilter
            {
                RowLimit = 999999999,
                UnderallocatedOnly = underAllocatedOnly
            };
            List<OrderFillmentResponse> quoteList = _orderFulfillmentService.GetOrderFulfillmentList(searchfilter).OrderFillment.ToList();

            //Turn list into excel
            string path = "";   //Will get transformed 
            string searchName = "";
            if (!string.IsNullOrEmpty(searchfilter.SearchString))
                searchName = "_Search_" + searchfilter.SearchString;
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_OrderFulfillmentList" + searchName + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<OFGridExportLine>(_orderFulfillmentService.MapSOLinesToExport(quoteList), ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/orderFulfillment/getPoAndInventoryAvailability")]
        public OrderFulfillmentAvailabilityListResponse GetOrderFulfillmentAvailability(int soLineId)
        {
            return _orderFulfillmentService.GetOrderFulfillmentAvailability(soLineId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/orderFulfillment/getPoAndInventoryAvailabilityExport")]
        public ExportResponse GetOrderFulfillmentAvailabilityExport(int soLineId)
        {
            //Convert to datatable (list does not work here)
            List<OrderFulfillmentAvailabilityResponse> inspectionList = _orderFulfillmentService.GetOrderFulfillmentAvailability(soLineId).OrderFulfillmentAvailabilityList.ToList();
            //DataTable dt = Sourceportal.Utilities.CreateExcelFile.ListToDataTable(inspectionList);

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_POAndInventoryAvailability_SO_" + soLineId + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument(inspectionList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpPost]
        [Route("api/orderFulfillment/setOrderFulfillmentQty")]
        public SetOrderFulfillmentQtyResponse SetOrderFulfillmentQty(OrderFulfillmentQtySetRequest orderFulfillmentQtySetRequest)
        {
            return _orderFulfillmentService.SetOrderFulfillmentQty(
                orderFulfillmentQtySetRequest.SoLineId, orderFulfillmentQtySetRequest.Id, orderFulfillmentQtySetRequest.Type, orderFulfillmentQtySetRequest.Quantity, orderFulfillmentQtySetRequest.IsDeleted);
        }

        [HttpPost]
        [Route("api/inventory/inbounddelivery")]
        public BaseResponse HandleInboundDelivery(InboundDeliverySapRequest request)
        {
            UserHelper.SetMiddlewareUser();
            BaseResponse response = new BaseResponse();

            try
            {
                response = _orderFulfillmentService.HandleInboundDelivery(request);
            }
            catch (Exception e)
            {
                return new BaseResponse { ErrorMessage = String.Format("Exception caught in Web Api Update: {0} | | STACK TRACE: {1}", e.Message, e.StackTrace), IsSuccess = false };
            }

            return response;
        }

        [HttpPost]
        [Route("api/inventory/logisticsexecution")]
        public BaseResponse HandleLogisticsExecution(LogisticsExecutionSapRequest request)
        {
            UserHelper.SetMiddlewareUser();
            BaseResponse response = new BaseResponse();

            try
            {
                response = _orderFulfillmentService.HandleLogisticsExecution(request);
            }
            catch (Exception e)
            {
                return new BaseResponse { ErrorMessage = String.Format("Exception caught in Web Api Update: {0} | | STACK TRACE: {1}", e.Message, e.StackTrace), IsSuccess = false };
            }

            return response;
        }

        [HttpPost]
        [Route("api/inventory/productionlot")]
        public BaseResponse HandleProductionLot(ProductionLotSapRequest request)
        {
            UserHelper.SetMiddlewareUser();
            BaseResponse response = new BaseResponse();

            try
            {
                //
            }
            catch (Exception e)
            {
                return new BaseResponse {  ErrorMessage = String.Format("Exception caught in Web Api Update: {0} | | STACK TRACE: {1}", e.Message, e.StackTrace), IsSuccess = false };
            }

            return response;
        }
    }
}
