using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.PurchaseOrders;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Comments;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using Sourceportal.Domain.Models.API.Responses.PurchaseOrders;
using SourcePortal.Services.CommonData;
using SourcePortal.Services.PurchaseOrders;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PurchaseOrdersController : ApiController
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ICommonDataService _commonDataService;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService, ICommonDataService commonDataService)
        {
            _purchaseOrderService = purchaseOrderService;
            _commonDataService = commonDataService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderDetails")]
        public PurchaseOrderResponse PurchaseOrderDetails(int purchaseOrderId, int versionId)
        {
            return _purchaseOrderService.GetPurchaseOrderDetails(purchaseOrderId, versionId, true);
        }

        [Authorize]
        [HttpPost]
        [Route("api/purchase-order/setPurchaseOrderDetails")]
        public PurchaseOrderDetailsSetResponse SetPurchaseOrderDetails(SetPurchaseOrderDetailsRequest setPoRequest)
        {
            return _purchaseOrderService.SetPurchaseOrderDetails(setPoRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/purchase-order/getPurchaseOrderList")]
        public PurchaseOrderListResponse PurchaseOrderList(SearchFilter searchfilter)
        {
            return _purchaseOrderService.GetPurchaseOrderList(searchfilter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderExportList")]
        public ExportResponse PurchaseOrderExportList(string searchString)
        {
            SearchFilter searchfilter = new SearchFilter { RowLimit = 999999999, SearchString = searchString };
            List<PurchaseOrderResponse> quoteList = _purchaseOrderService.GetPurchaseOrderList(searchfilter).PurchaseOrders.ToList();

            //Turn list into excel
            string path = "";   //Will get transformed 
            string searchName = "";
            if (!string.IsNullOrEmpty(searchfilter.SearchString))
                searchName = "_Search_" + searchfilter.SearchString;
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_PurchaseOrdersList" + searchName + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<PurchaseOrderResponse>(quoteList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderLines")]
        public GetPurchaseOrderLinesResponse PurchaseOrderLines(int poId, int poVersionId, int rowOffset = 0, int rowLimit = 100000, string sortCol = "", bool descSort = false)
        {
            SearchFilter searchfilter = new SearchFilter
            {
                SearchString = "",
                RowOffset = rowOffset,
                RowLimit = rowLimit,
                SortCol = sortCol,
                DescSort = descSort
            };
            return _purchaseOrderService.GetPurchaseOrderLines(poId, poVersionId, searchfilter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderExportLines")]
        public ExportResponse PurchaseOrderExportLines(int poId, int poVersionId)
        {
            //Aquire full list
            SearchFilter searchfilter = new SearchFilter { RowLimit = 999999999 };
            List<PurchaseOrderLineDetail> srcList = _purchaseOrderService.GetPurchaseOrderLines(poId, poVersionId, searchfilter).POLinesResponse;

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_PurchaseOrder[" + poId + "]V[" + poVersionId + "]_PartsList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<PurchaseOrderLineDetail>(srcList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpPost]
        [Route("api/purchase-order/setPurchaseOrderLine")]
        public PurchaseOrderLineDetail SetPurchaseOrderLine(SetPurchaseOrderLineRequest setPurchaseOrderLineRequest)
        {
            return _purchaseOrderService.SetPurchaseOrderLine(setPurchaseOrderLineRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/purchase-order/deletePurchaseOrderLines")]
        public PurchaseOrderLinesDeleteResponse DeletePurchaseOrderLiness(PurchaseOrderLineDetail[] poLines)
        {
            return _purchaseOrderService.DeletePurchaseOrderLines(poLines.ToList().Select(x => x.POLineId).ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderExtras")]
        public PurchaseOrderExtraResponse PurchaseOrderExtras(int PurchaseOrderId, int POVersionId, int RowOffset, int RowLimit)
        {
            return _purchaseOrderService.GetPurchaseOrderExtra(PurchaseOrderId, POVersionId, RowOffset, RowLimit);
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderExtrasExport")]
        public ExportResponse PurchaseOrderExtrasExport(int PurchaseOrderId, int POVersionId)
        {
            //Aquire full list
            List<ExtraListResponse> srcList = _purchaseOrderService.GetPurchaseOrderExtra(PurchaseOrderId, POVersionId, 0, 999999999).ExtraListResponse;

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_PurchaseOrder[" + PurchaseOrderId + "]V[" + POVersionId + "]_ExtraList.xlsx";
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
        [Route("api/purchase-order/setPurchaseOrderExtra")]
        public SetPurchaseOrderExtraResponse SetPurchaseOrderExtra(SetPurchaseOrderExtraRequest setPurchaseOrderExtraRequest)
        {
            return _purchaseOrderService.SetPurchaseOrderExtra(setPurchaseOrderExtraRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/purchase-order/deletePurchaseOrderExtras")]
        public PurchaseOrderExtraDeleteResponse DeletePurchaseOrderExtras(ExtraListResponse[] poExtras)
        {
            return _purchaseOrderService.DeletePurchaseOrderExtras(poExtras.ToList().Select(x => x.POExtraId).ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPaymentTermsList")]
        public PaymentTermsListResponse GetPaymentTermsList(int paymentTermId)
        {
            return _commonDataService.GetPaymentTerms(paymentTermId);
        }


        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getCurrenciesList")]
        public CurrencyListResponse GetCurrencyList()
        {
            return _purchaseOrderService.GetCurrencies();
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderObjectTypeId")]
        public ObjectTypeIdResponse GetPurchaseOrderObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.Purchaseorder;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPOLineObjectTypeId")]
        public ObjectTypeIdResponse GetPOLineObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.PurchaseorderLine;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPOExtraObjectTypeId")]
        public ObjectTypeIdResponse GetPOExtraObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.PurchaseorderExtra;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderCommentTypeIds")]
        public CommentTypeIdsResponse GetPOCommentTypeIds()
        {
            var response = new CommentTypeIdsResponse();

            var commentTypeIds = new List<CommentTypeMap>();

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.SalesPurchasing,
                TypeName = "Sales"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.PurchasingPurchasing,
                TypeName = "Purchasing"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.WarehousePurchasing,
                TypeName = "Warehouse"
            });

            response.CommentTypeIds = commentTypeIds;
            response.IsSuccess = true;
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("api/purchase-order/newPurchaseOrderFromFlaggedSet")]
        public PurchaseOrderDetailsSetResponse PurchaseOrderFromFlaggedSet(
            SetPurchaseItemsFlaggedRequest setPurchaseItemsFlaggedRequest)
        {
            return _purchaseOrderService.PurchaseOrderFromFlaggedSet(setPurchaseItemsFlaggedRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getMfr")]
        public ItemMfrResponse GetManufacturerItem(int itemId)
        {
            return _purchaseOrderService.GetManufactuerItem(itemId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getStatusByPurchaseOrder")]
        public StatusListResponse GetStatusByPurchaseOrder()
        {
            return _commonDataService.GetStatusesResponse(ObjectType.Purchaseorder);
        }

        [Authorize]
        [HttpPost]
        [Route("api/purchase-order/sync")]
        public SyncResponse SyncPurchaseOrder(int poId, int poVersionId)
        {
            return _purchaseOrderService.Sync(poId, poVersionId);
        }
        
        [HttpPost]
        [Route("api/purchase-order/updateExternalId")]
        public BaseResponse UpdateExternalId(SetPurchaseOrderSapDataRequest request)
        {
            UserHelper.SetMiddlewareUser();

            try
            {
                _purchaseOrderService.SetPurchaseOrderSapData(request);
            }
            catch (Exception ex)
            {
                throw new GlobalApiException(ex.Message);
            }

            return new BaseResponse();
        }

        [HttpPost]
        [Route("api/purchase-order/updatePurchaseOrder")]
        public BaseResponse UpdatePurchaseOrderFromSap(PurchaseOrderIncomingSapResponse request)
        {
            UserHelper.SetMiddlewareUser();
            return _purchaseOrderService.HandlingIncomingPurchaseOrderSapUpdate(request);
        }

        [HttpGet]
        [Route("api/purchase-order/validateSync")]
        public string ValidateSync(int purchaseOrderId, int versionId)
        {
            return _purchaseOrderService.ValidateSync(purchaseOrderId, versionId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/purchase-order/getPurchaseOrderLineByAccountId")]
        public PurchaseOrderLineHistoryResponse GetPurchaseOrderByAccountId(int accountId,int? contactId = null)
        {
            return _purchaseOrderService.GetPurchaseOrderByAccountId(accountId,contactId);
        }
    }
}
