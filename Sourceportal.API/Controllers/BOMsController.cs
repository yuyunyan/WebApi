using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.BOMs;
using Sourceportal.Domain.Models.API.Responses.BOMs;
using SourcePortal.Services.BOMs;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Comments;
using Sourceportal.Domain.Models.API.Responses.CommonData;

namespace Sourceportal.API.Controllers
{
    public class BOMsController : ApiController
    {
        private readonly IBOMsService _BOMsService;

        public BOMsController(IBOMsService BOMsService)
        {
            _BOMsService = BOMsService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/boms/uploadBOM")]
        public UploadBOMResponse UploadBOMXlsFile()
        {
            return _BOMsService.UploadListXlsFile();   //BOM ListTypeID
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getBOMList")]
        public BOMListResponse GetBOMList(string searchString, int rowOffset=0, int rowLimit=50, string sortBy="", bool descSort=false)
        {
            SearchFilter searchfilter = new SearchFilter
            {
                SearchString = searchString,
                RowOffset = rowOffset,
                RowLimit = rowLimit,
                SortCol = sortBy,
                DescSort = descSort
            };
            return _BOMsService.GetBOMList(searchfilter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getBOMExportList")]
        public ExportResponse GetBOMExportList(string searchString)
        {
            //Aquire full list
            SearchFilter filter = new SearchFilter() { SearchString = searchString, RowLimit = 999999999 };
            List<BOMList> list = _BOMsService.GetBOMList(filter).BomList;
            //Turn list into excel
            string path = "";   //Will get transformed
            string searchName = "";

            //Add search parameter to file name
            if (!string.IsNullOrEmpty(filter.SearchString))
                searchName = "_Search_" + filter.SearchString;

            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_BOMList" + searchName + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<BOMList>(list, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpPost]
        [Route("api/boms/bomProcessMatch")]
        public BomProcessMatchResponse BomProcessMatch(ProcessMatchRequest processMatchRequest)
        {
            return _BOMsService.ProcessMatch(processMatchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getSalesOrder")]
        public SalesOrderResponse GetSalesOrder([FromUri]  BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetSalesOrder(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getInventory")]
        public InventoryBomResponse GetInventory([FromUri]  BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetInventory(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getOutsideOffers")]
        public OutsideOffersBomResponse GetOutsideOffers([FromUri]  BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetOutsideOffers(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getVendorQuotes")]
        public VendorQuotesBomResponse GetVendorQuotes([FromUri]  BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetVendorQuotes(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getResultSummary")]
        public ResultSummaryResponse GetResultSummary([FromUri] BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetResultSummary(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getPurchaseOrders")]
        public PurchaseOrderBomResponse GetPurchaseOrders([FromUri] BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetPurchaseOrders(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getCustomerQuotes")]
        public CustomerQuoteBomResponse GetCustomerQuotes([FromUri] BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetCustomerQuotes(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getCustomerRfqs")]
        public CustomerRFQBomResponse GetCustomerRfqs([FromUri] BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetCustomerRfqs(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getEMSs")]
        public EMSBomResponse GetEMSs([FromUri] BomSearchRequest bomSearchRequest)
        {
            return _BOMsService.GetEMSs(bomSearchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getBOMObjectTypeId")]
        public ObjectTypeIdResponse GetBOMObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.ItemList;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getBOMCommentTypeIds")]
        public CommentTypeIdsResponse GetBOMCommentTypeIds()
        {
            var response = new CommentTypeIdsResponse();

            var commentTypeIds = new List<CommentTypeMap>();

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.ItemList,
                TypeName = "Comment"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.SalesItemList,
                TypeName = "Sales"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.PurchaseItemList,
                TypeName = "Purchasing"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.WarehouseItemList,
                TypeName = "Warehouse"
            });

            response.CommentTypeIds = commentTypeIds;
            response.IsSuccess = true;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getPartSearchResult")]
        public PartSearchResultResponse GetPartSearchResult(string searchString,string searchType)
        {
            return _BOMsService.GetPartSearchResult(searchString, searchType);
        }

        [Authorize]
        [HttpGet]
        [Route("api/boms/getAvailabilityPart")]
        public PartSearchAvailabilityResponse GetAvailabilityPart(int itemId)
        {
            return _BOMsService.GetAvailabilityPart(itemId);
        }


    }
}