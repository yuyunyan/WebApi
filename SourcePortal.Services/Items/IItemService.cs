using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Responses.Items;
using Sourceportal.Domain.Models.API.Requests.Items;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Domain.Models.API.Responses;

namespace SourcePortal.Services.Items
{
    public interface IItemService
    {
        IList<ItemStatus> GetItemStatuses();
        int GetItemIdFromExternal(string externalId);
        ItemCommodityListResponse GetItemCommodityList();
        ItemGroupListResponse GetItemGroupList();
        ManufacturerListResponse GetManufacturerList(string searchText);
        ItemExtraListResponse GetItemExtraList();
        ItemListResponse GetItemList(SearchFilter searchfilter);
        ItemResponse GetItemDetails(int ItemID);
        SetItemDetailsResponse SetItemDetails(SetItemDetailsRequest itemdetails);
        void DeleteItem(int itemId);
        ItemsResponse GetItems();
        PartSearchResponse GetSuggestions(string searchString);
        void SetItemExternalId(SetExternalIdRequest request);
        SyncResponse Sync(int itemId);
        int CreateManufacturer(SetManufacturerRequest model);
        ItemPurchaseOrdersListResponse GetItemPurchaseOrders(ItemPOsListGetRequest itemPOsListRequest);
        ItemTechnicalDataResponse GetTechnicalData(int itemId);
        ItemInventoryResponse GetItemInventory(int ItemID, bool ExcludePo, int RowOffset, int RowLimit, string SortCol, bool DescSort);
        ItemAvailabilityResponse GetItemAvailability(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort);
        ItemSalesOrdersResponse GetItemSalesOrders(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort);
        ItemQuotesResponse GetItemQuotes(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort);
    }
}
