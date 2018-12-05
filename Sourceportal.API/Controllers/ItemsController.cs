using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sourceportal.DB.Items;
using SourcePortal.Services.Items;
using Sourceportal.Domain.Models.API.Responses.Items;
using Sourceportal.Domain.Models.API.Requests.Items;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Domain.Models.API.Responses.Sync;

namespace Sourceportal.API.Controllers
{
    public class ItemsController : ApiController
    {
        private readonly IItemService _itemService;
        private readonly IItemsIhsRepository _itemsIhsRepository;

        public ItemsController(IItemService itemservice, IItemsIhsRepository itemsIhsRepository)
        {
            _itemService = itemservice;
            _itemsIhsRepository = itemsIhsRepository;
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemCommodityList")]
        public ItemCommodityListResponse CommodityList()
        {
            return _itemService.GetItemCommodityList();
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemGroupList")]
        public ItemGroupListResponse GroupList()
        {
            return _itemService.GetItemGroupList();
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemPurchaseOrders")]
        public ItemPurchaseOrdersListResponse ItemPurchaseOrders(int RowOffset, int RowLimit, string SortBy, bool DescSort, int itemId )
        {
            ItemPOsListGetRequest itemPOsListGetRequest = new ItemPOsListGetRequest
            {
                ItemID = itemId,
                RowOffset = RowOffset,
                RowLimit = RowLimit,
                SortBy = SortBy,
                DescSort = DescSort
            };
            return _itemService.GetItemPurchaseOrders(itemPOsListGetRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/items/deleteItem")]
        public void DeleteItem(int itemId)
        {
            _itemService.DeleteItem(itemId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemStatuses")]
        public IList<ItemStatus> GetItemStatuses()
        {
            return _itemService.GetItemStatuses();
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getManufacturersList")]
        public ManufacturerListResponse ManufacturersList(string searchText)
        {
            return _itemService.GetManufacturerList(searchText);
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemExtraList")]
        public ItemExtraListResponse ItemExtraList()
        {
            return _itemService.GetItemExtraList();
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemsList")]
        public ItemListResponse ItemList(string SearchString, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            SearchFilter searchfilter = new SearchFilter
            {
                SearchString = SearchString,
                RowOffset = RowOffset,
                RowLimit = RowLimit,
                SortCol = SortCol,
                DescSort = DescSort
            };
            return _itemService.GetItemList(searchfilter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemsExportList")]
        public ExportResponse ItemExportList(string searchString)
        {
            //Aquire full list
            SearchFilter filter = new SearchFilter() { SearchString = searchString, RowLimit = 999999999 };
            List<ItemResponse> list = _itemService.GetItemList(filter).Items.ToList();
            //Turn list into excel
            string path = "";   //Will get transformed
            string searchName = "";

            //Add search parameter to file name
            if (!string.IsNullOrEmpty(filter.SearchString))
                searchName = "_Search_" + filter.SearchString;

            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_ItemsList" + searchName + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<ItemResponse>(list, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemDetails")]
        public ItemResponse ItemDetails(int ItemID)
        {
            return _itemService.GetItemDetails(ItemID);
        }

        [Authorize]
        [HttpPost]
        [Route("api/items/setItemDetails")]
        public SetItemDetailsResponse SetItemDetails(SetItemDetailsRequest itemdetailsrequest)
        {
            return _itemService.SetItemDetails(itemdetailsrequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItems")]
        public ItemsResponse GetItems()
        {
            return _itemService.GetItems();
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getSuggestions")]
        public PartSearchResponse GetItems(string searchString)
        {
            return _itemService.GetSuggestions(searchString);
        }

        //[Authorize]
        [HttpPost]
        [Route("api/items/updateExternalId")]
        public BaseResponse SetItemExternalId(SetExternalIdRequest request)
        {
            UserHelper.SetMiddlewareUser();

            try
            {
                _itemService.SetItemExternalId(request);
            }
            catch (Exception ex)
            {
                throw new GlobalApiException(ex.Message);
            }

            return new BaseResponse();
        }

        [Authorize]
        [HttpPost]
        [Route("api/items/createManufacturer")]
        public int CreateManufacturer(SetManufacturerRequest model)
        {
           return _itemService.CreateManufacturer(model);
        }

        [Authorize]
        [HttpPost]
        [Route("api/items/sync")]
        public SyncResponse SyncItem(int itemId)
        {
            return _itemService.Sync(itemId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getTechnicalData")]
        public ItemTechnicalDataResponse GetTechnicalData(int itemId)
        {
            return _itemService.GetTechnicalData(itemId);
        }

        [Authorize]
        [HttpGet]

        [Route("api/items/getItemInventory")]
        public ItemInventoryResponse ItemInventory(int ItemID, bool ExcludePo, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            return _itemService.GetItemInventory(ItemID, ExcludePo, RowOffset, RowLimit, SortCol, DescSort);
		}

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemAvailability")]
        public ItemAvailabilityResponse ItemAvailability(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            return _itemService.GetItemAvailability(ItemID, RowOffset, RowLimit, SortCol, DescSort);
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemSalesOrders")]
        public ItemSalesOrdersResponse ItemSalesOrders(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)

        {
            return _itemService.GetItemSalesOrders(ItemID, RowOffset, RowLimit, SortCol, DescSort);
        }

        [Authorize]
        [HttpGet]
        [Route("api/items/getItemQuotes")]
        public ItemQuotesResponse ItemQuotes(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            return _itemService.GetItemQuotes(ItemID, RowOffset, RowLimit, SortCol, DescSort);

        }
    }
}